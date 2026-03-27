using System.ComponentModel.DataAnnotations;

namespace ConstructionServicesApp.Models
{
    public class Inventory
    {
        [Key]
        public int ItemID { get; set; }

        public int ServiceID { get; set; }
        public Service Service { get; set; }

        public string ItemName { get; set; }

        public int Quantity { get; set; }

        public string Status { get; set; }
    }
}
