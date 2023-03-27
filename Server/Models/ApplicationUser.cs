using HealthyHands.Shared.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthyHands.Server.Models
{
    /// <summary>
    /// The application user.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        [Column(Order = 999)]
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
        /// <summary>
        /// Gets or sets the birth day.
        /// </summary>
        public DateTime? BirthDay { get; set; }
        /// <summary>
        /// Gets or sets the user weights.
        /// </summary>
        public List<UserWeight> UserWeights { get; set; } = new List<UserWeight>();
        /// <summary>
        /// Gets or sets the user meals.
        /// </summary>
        public List<UserMeal> UserMeals { get; set; } = new List<UserMeal>();
        /// <summary>
        /// Gets or sets the user workouts.
        /// </summary>
        public List<UserWorkout> UserWorkouts { get; set; } = new List<UserWorkout>();
    }
}