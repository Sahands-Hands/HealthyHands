// Filename: UserWeightDto.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyHands.Shared.Models
{
    /// <summary>
    /// The user weight dto.
    /// </summary>
    public class UserWeightDto
    {
        /// <summary>
        /// Gets or sets the user weight id.
        /// </summary>
        public string UserWeightId { get; set; } = string.Empty;
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
        public string? ApplicationUserId { get; set; } = string.Empty;
    }
}
