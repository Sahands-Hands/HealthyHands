namespace HealthyHands.Shared.Models
{
    public class UserDto
    {
        public string Id { get; set; } = String.Empty;
        public string UserName { get; set; } = String.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? Height { get; set; }
        public int? Gender { get; set; }
        public int? ActivityLevel { get; set; }
        public DateTime? BirthDay { get; set; }
        public List<UserMeal> UserMeals { get; set; } = new List<UserMeal>();
        public List<UserWeight> UserWeights { get; set; } = new List<UserWeight>();
        public List<UserWorkout> UserWorkouts { get; set; } = new List<UserWorkout>();
    }
}
