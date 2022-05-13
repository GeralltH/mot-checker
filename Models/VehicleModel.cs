namespace MOTChecker.Models
{
    public class VehicleModel
    {
        public string Registration { get; set; }
        public string Make { get; set; }

        public string Model { get; set; }

        public DateOnly MotExpiryDate { get; set; }

        public int LastMotMileage { get; set; }

    }
}
