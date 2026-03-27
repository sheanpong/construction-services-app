namespace ConstructionServicesApp.Models
{
    public class BookingDetail
    {
        public int BookingDetailID { get; set; }

        public int BookingID { get; set; }
        public Booking Booking { get; set; }

        public int ServiceID { get; set; }
        public Service Service { get; set; }

        public int HoursRendered { get; set; }

        public decimal Rate { get; set; }

        public decimal Amount { get; set; }
    }
}
