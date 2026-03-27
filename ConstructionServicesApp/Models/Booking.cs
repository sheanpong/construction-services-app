namespace ConstructionServicesApp.Models
{
    using System;
    using System.Collections.Generic;

    public class Booking
    {
        public int BookingID { get; set; }

        public int ClientID { get; set; }
        public Client Client { get; set; }

        public DateTime BookingDate { get; set; }

        public string Status { get; set; } // Pending, Completed, Cancelled

        public List<BookingDetail> BookingDetails { get; set; }
    }
}
