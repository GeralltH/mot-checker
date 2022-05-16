namespace MOTChecker.Models
{
    public class VehicleModel
    {
        public string Registration { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string FuelType { get; set; }
        public string PrimaryColour { get; set; }
        public string? MotTestExpiryDate { get; set; }
        public MotModel[]? MotTests { get; set; }

    }
}
