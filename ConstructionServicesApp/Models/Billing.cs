namespace ConstructionServicesApp.Models
{
    using System;

    public class Billing
    {
        public int BillingID { get; set; }

        public int BookingID { get; set; }
        public Booking Booking { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime DateGenerated { get; set; }

        public string PaymentStatus { get; set; } // Unpaid, Paid
    }
}
