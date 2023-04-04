// Filename: UserWeightDto.cs

namespace HealthyHands.Shared.Models
{
    /// <summary>
    /// The user weight.
    /// </summary>
    public class UserWeight
    {
        /// <summary>
        /// Gets or sets the user weight id.
        /// </summary>
        public string UserWeightId { get; set; }
        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        public double Weight { get; set; }
        /// <summary>
        /// Gets or sets the weight date.
        /// </summary>
        public DateTime WeightDate { get; set; }
        /// <summary>
        /// Gets or sets the application user id.
        /// </summary>
        public string ApplicationUserId { get; set; }
    }
}
