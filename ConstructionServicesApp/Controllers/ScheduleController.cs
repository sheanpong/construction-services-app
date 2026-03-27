using Microsoft.AspNetCore.Mvc;

namespace ConstructionServicesApp.Controllers
{
    using ConstructionServicesApp.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class ScheduleController : Controller
    {
        private readonly AppDbContext _context;

        public ScheduleController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            DateTime startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            DateTime endOfWeek = startOfWeek.AddDays(6);

            var bookings = _context.Bookings
                .Include(b => b.Client)
                .Where(b => b.BookingDate >= startOfWeek && b.BookingDate <= endOfWeek)
                .ToList();

            return View(bookings);
        }
    }
}
