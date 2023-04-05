// Filename: UserWorkoutDto.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyHands.Shared.Models
{
    /// <summary>
    /// The user workout dto.
    /// </summary>
    public class UserWorkoutDto
    {
        /// <summary>
        /// Gets or sets the user workout id.
        /// </summary>
        public string UserWorkoutId { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the workout name.
        /// </summary>
        public string WorkoutName { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the workout type.
        /// </summary>
        public int WorkoutType { get; set; }
        /// <summary>
        /// Gets or sets the intensity.
        /// </summary>
        public int Intensity { get; set; } = 0;
        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        public int Length { get; set; } = 0;
        /// <summary>
        /// Gets or sets the workout date.
        /// </summary>
        public DateTime WorkoutDate { get; set; }
        /// <summary>
        /// Gets or sets the calories burned.
        /// </summary>
        public int? CaloriesBurned { get; set; }
        /// <summary>
        /// Gets or sets the application user id.
        /// </summary>
        public string? ApplicationUserId { get; set; } = string.Empty;
    }
}
