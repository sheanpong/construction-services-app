namespace ConstructionServicesApp.Models
{
    using System;

    public class Payment
    {
        public int PaymentID { get; set; }

        public int BillingID { get; set; }
        public Billing Billing { get; set; }

        public decimal AmountPaid { get; set; }

        public DateTime PaymentDate { get; set; }

        public string PaymentMethod { get; set; }
    }
}
