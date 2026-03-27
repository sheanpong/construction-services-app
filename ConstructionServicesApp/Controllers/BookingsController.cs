using ConstructionServicesApp.Models;
using ConstructionServicesApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionServicesApp.Controllers
{
    public class BookingsController : Controller
    {
        private readonly AppDbContext _context;

        public BookingsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Bookings.Include(b => b.Client);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Client)
                .FirstOrDefaultAsync(m => m.BookingID == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Clients = _context.Clients.ToList();
            ViewBag.Services = _context.Services.ToList();

            // Return a view model with a list for multi-service selection
            var vm = new BookingViewModel
            {
                Services = new List<ServiceSelection>()
            };
            return View(vm);
        }

        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingViewModel vm)
        {
            // 🔒 Prevent double booking
            bool exists = _context.Bookings.Any(b => b.BookingDate == vm.BookingDate);
            if (exists)
            {
                ModelState.AddModelError("", "Date already booked!");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Clients = _context.Clients.ToList();
                ViewBag.Services = _context.Services.ToList();
                return View(vm);
            }

            // 🧾 Save Booking
            var booking = new Booking
            {
                ClientID = vm.ClientID,
                BookingDate = vm.BookingDate,
                Status = "Pending"
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            decimal total = 0;

            // 📋 Save Booking Details (multi-service)
            foreach (var item in vm.Services)
            {
                var amount = item.Hours * item.Rate;
                var detail = new BookingDetail
                {
                    BookingID = booking.BookingID,
                    ServiceID = item.ServiceID,
                    HoursRendered = item.Hours,
                    Rate = item.Rate,
                    Amount = amount
                };

                total += amount;
                _context.BookingDetails.Add(detail);
            }

            await _context.SaveChangesAsync();

            // 💵 Generate Billing
            var billing = new Billing
            {
                BookingID = booking.BookingID,
                TotalAmount = total,
                DateGenerated = DateTime.Now,
                PaymentStatus = "Unpaid"
            };
            _context.Billings.Add(billing);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["ClientID"] = new SelectList(_context.Clients, "ClientID", "ContactNumber", booking.ClientID);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingID,ClientID,BookingDate,Status")] Booking booking)
        {
            if (id != booking.BookingID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientID"] = new SelectList(_context.Clients, "ClientID", "ContactNumber", booking.ClientID);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Client)
                .FirstOrDefaultAsync(m => m.BookingID == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.BookingDetails)
                .FirstOrDefaultAsync(b => b.BookingID == id);

            if (booking != null)
            {
                if (booking.BookingDetails?.Any() == true)
                {
                    _context.BookingDetails.RemoveRange(booking.BookingDetails);
                }
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingID == id);
        }
    }
}
