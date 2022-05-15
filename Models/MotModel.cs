namespace MOTChecker.Models
{
    public class MotModel
    {
        public string completedDate { get; set; }
        public string testResult { get; set; }
        public string? expiryDate { get; set; }
        public int odometerValue { get; set; }
        public string odometerUnit { get; set; }
        public string motTestNumber { get; set; }
        public FaultModel[]? rfrAndComments { get; set; }
    }
}
