namespace ConstructionServicesApp.Models.ViewModels
{
    using System;
    using System.Collections.Generic;

    public class BookingViewModel
    {
        public int ClientID { get; set; }
        public DateTime BookingDate { get; set; }

        public List<ServiceSelection> Services { get; set; }

        public decimal TotalAmount { get; set; }
    }

    public class ServiceSelection
    {
        public int ServiceID { get; set; }
        public int Hours { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
    }
}
