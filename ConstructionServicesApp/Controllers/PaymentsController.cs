using ConstructionServicesApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class PaymentsController : Controller
{
    private readonly AppDbContext _context;

    public PaymentsController(AppDbContext context)
    {
        _context = context;
    }

    // 📋 List unpaid billings
    public async Task<IActionResult> Index()
    {
        var billings = await _context.Billings
            .Include(b => b.Booking)
            .ThenInclude(b => b.Client)
            .Where(b => b.PaymentStatus == "Unpaid")
            .ToListAsync();

        return View(billings);
    }

    public IActionResult Pay(int id)
    {
        var billing = _context.Billings.Find(id);
        return View(billing);
    }

    [HttpPost]
    public async Task<IActionResult> Pay(int id, decimal amount, string method)
    {
        var payment = new Payment
        {
            BillingID = id,
            AmountPaid = amount,
            PaymentDate = DateTime.Now,
            PaymentMethod = method
        };

        _context.Payments.Add(payment);

        var billing = await _context.Billings.FindAsync(id);
        billing.PaymentStatus = "Paid";

        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

}



