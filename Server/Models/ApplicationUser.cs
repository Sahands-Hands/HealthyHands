using HealthyHands.Shared.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthyHands.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Column(Order = 999)]
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? Height { get; set; }
        public int? Gender { get; set; }
        public int? ActivityLevel { get; set; }
        public DateTime? BirthDay { get; set; }
        public List<UserWeight> UserWeights { get; set; } = new List<UserWeight>();
        public List<UserMeal> UserMeals { get; set; } = new List<UserMeal>();
        public List<UserWorkout> UserWorkouts { get; set; } = new List<UserWorkout>();
    }
}