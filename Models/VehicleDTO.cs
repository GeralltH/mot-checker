using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MOTChecker.Models
{
    public class VehicleDTO
    {
        public string Registration { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Colour { get; set; }
        [DisplayName("MOT Expiry Date")]
        public string MotExpiryDate { get; set; }
        [DisplayName("MOT Mileage")]
        public string LastMotMileage { get; set; }
    }
}
