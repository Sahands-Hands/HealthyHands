namespace HealthyHands.Shared.Models
{
    /// <summary>
    /// The user workout.
    /// </summary>
    public class UserWorkout
    {
        /// <summary>
        /// Gets or sets the user workout id.
        /// </summary>
        public string UserWorkoutId { get; set; }
        /// <summary>
        /// Gets or sets the workout name.
        /// </summary>
        public string WorkoutName { get; set; }
        /// <summary>
        /// Gets or sets the workout type.
        /// </summary>
        public int WorkoutType { get; set; }
        /// <summary>
        /// Gets or sets the intensity.
        /// </summary>
        public int Intensity { get; set; }
        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// Gets or sets the workout date.
        /// </summary>
        public DateTime WorkoutDate { get; set; }
        /// <summary>
        /// Gets or sets the calories burned.
        /// </summary>
        public int? CaloriesBurned { get; set; }

    }
}
