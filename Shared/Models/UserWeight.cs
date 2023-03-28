namespace HealthyHands.Shared.Models
{
    public class UserWeight
    {
        public string UserWeightId { get; set; }
        public double Weight { get; set; }
        public DateTime WeightDate { get; set; }
        /// <summary>
        /// Gets or sets the application user id.
        /// </summary>
        public string ApplicationUserId { get; set; }
    }
}
