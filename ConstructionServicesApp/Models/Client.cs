namespace ConstructionServicesApp.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Client
    {
        public int ClientID { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string ContactNumber { get; set; }

        public string Address { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}
