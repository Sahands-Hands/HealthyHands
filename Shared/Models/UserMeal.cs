namespace HealthyHands.Shared.Models
{
    /// <summary>
    /// The user meal.
    /// </summary>
    public class UserMeal
    {
        /// <summary>
        /// Gets or sets the user meal id.
        /// </summary>
        public string UserMealId { get; set; }
        /// <summary>
        /// Gets or sets the meal name.
        /// </summary>
        public string MealName { get; set; }
        /// <summary>
        /// Gets or sets the meal date.
        /// </summary>
        public DateTime MealDate { get; set; }
        /// <summary>
        /// Gets or sets the calories.
        /// </summary>
        public int? Calories { get; set; }
        /// <summary>
        /// Gets or sets the protein.
        /// </summary>
        public int? Protein { get; set; }
        /// <summary>
        /// Gets or sets the carbs.
        /// </summary>
        public int? Carbs { get; set; }
        /// <summary>
        /// Gets or sets the fat.
        /// </summary>
        public int? Fat { get; set; }
        /// <summary>
        /// Gets or sets the sugar.
        /// </summary>
        public int? Sugar { get; set; }
        public string ApplicationUserId { get; set; } = string.Empty;
    }
}
