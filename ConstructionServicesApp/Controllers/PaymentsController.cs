using ConstructionServicesApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class PaymentsController : Controller
{
    private readonly AppDbContext _context;

    public PaymentsController(AppDbContext context)
    {
        _context = context;
    }

    // 📋 List payment history
    public async Task<IActionResult> Index()
    {
        var payments = await _context.Payments
            .Include(p => p.Billing)
            .ThenInclude(b => b.Booking)
            .ThenInclude(b => b.Client)
            .ToListAsync();

        return View(payments);
    }

    public IActionResult Create()
    {
        ViewData["BillingID"] = new SelectList(
            _context.Billings
                .Include(b => b.Booking)
                .ThenInclude(b => b.Client)
                .Where(b => b.PaymentStatus == "Unpaid"),
            "BillingID",
            "BillingID"
        );
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Payment payment)
    {
        if (ModelState.IsValid)
        {
            _context.Payments.Add(payment);

            var billing = await _context.Billings.FindAsync(payment.BillingID);
            if (billing != null)
            {
                billing.PaymentStatus = "Paid";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        ViewData["BillingID"] = new SelectList(
            _context.Billings.Include(b => b.Booking).ThenInclude(b => b.Client),
            "BillingID",
            "BillingID",
            payment.BillingID
        );
        return View(payment);
    }

}



