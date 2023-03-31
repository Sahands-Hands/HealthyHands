// Filename: UserDto.cs

namespace HealthyHands.Shared.Models
{
    /// <summary>
    /// The user dto.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; } = String.Empty;
        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string UserName { get; set; } = String.Empty;
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string? FirstName { get; set; }
        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string? LastName { get; set; }
        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public int? Height { get; set; }
        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        public int? Gender { get; set; }
        /// <summary>
        /// Gets or sets the activity level.
        /// </summary>
        public int? ActivityLevel { get; set; }
        
        public int? WeightGoal { get; set; }
        /// <summary>
        /// Gets or sets the daily calorie goal
        /// </summary>
        public int? CalorieGoal { get; set; }
        /// <summary>
        /// Gets or sets the birth day.
        /// </summary>
        public DateTime? BirthDay { get; set; }
        /// <summary>
        /// Gets or sets the user meals.
        /// </summary>
        public List<UserMeal> UserMeals { get; set; } = new List<UserMeal>();
        /// <summary>
        /// Gets or sets the user weights.
        /// </summary>
        public List<UserWeight> UserWeights { get; set; } = new List<UserWeight>();
        /// <summary>
        /// Gets or sets the user workouts.
        /// </summary>
        public List<UserWorkout> UserWorkouts { get; set; } = new List<UserWorkout>();
    }
}
