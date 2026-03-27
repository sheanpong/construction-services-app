namespace ConstructionServicesApp.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Service
    {
        public int ServiceID { get; set; }

        [Required]
        public string ServiceName { get; set; }

        [Required]
        [Range(1, 10000)]
        public decimal HourlyRate { get; set; }
    }
}
