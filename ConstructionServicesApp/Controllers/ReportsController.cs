using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConstructionServicesApp.Models;
using ConstructionServicesApp.Models.ViewModels;

public class ReportsController : Controller
{
    private readonly AppDbContext _context;

    public ReportsController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index(DateTime? startDate, DateTime? endDate)
    {
        var data = _context.Billings
            .Include(b => b.Booking)
            .ThenInclude(b => b.Client)
            .Select(b => new ReportViewModel
            {
                ClientName = b.Booking.Client.FullName,
                BookingDate = b.Booking.BookingDate,
                TotalAmount = b.TotalAmount,
                PaymentStatus = b.PaymentStatus
            });

        if (startDate.HasValue && endDate.HasValue)
        {
            data = data.Where(r => r.BookingDate >= startDate && r.BookingDate <= endDate);
        }

        return View(data.ToList());
    }
}