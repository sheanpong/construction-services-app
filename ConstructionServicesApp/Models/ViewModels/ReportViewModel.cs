namespace ConstructionServicesApp.Models.ViewModels
{
    using System;

    public class ReportViewModel
    {
        public string ClientName { get; set; }
        public DateTime BookingDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; }
    }
}
