// Filename: UserMealDto.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyHands.Shared.Models
{
    /// <summary>
    /// The user meal dto.
    /// </summary>
    public class UserMealDto
    {
        /// <summary>
        /// Gets or sets the meal id.
        /// </summary>
        public string UserMealId { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the meal name.
        /// </summary>
        public string MealName { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the meal consumption time.
        /// </summary>
        public DateTime MealDate { get; set; }
        /// <summary>
        /// Gets or sets the calorie amount.
        /// </summary>
        public int? Calories { get; set; }
        /// <summary>
        /// Gets or sets the protein amount.
        /// </summary>
        public int? Protein { get; set; }
        /// <summary>
        /// Gets or sets the carbs amount.
        /// </summary>
        public int? Carbs { get; set; }
        /// <summary>
        /// Gets or sets the fat amount.
        /// </summary>
        public int? Fat { get; set; }
        /// <summary>
        /// Gets or sets the sugar amount.
        /// </summary>
        public int? Sugar { get; set; }
        /// <summary>
        /// Gets or sets the application user id.
        /// </summary>
        public string ApplicationUserId { get; set; } = string.Empty;
    }
}
