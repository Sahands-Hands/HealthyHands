using HealthyHands.Shared.Models;

namespace HealthyHands.Client.HttpRepository.MealHttpRepository
{
    /// <summary>
    /// Meals interface that contains five  asynchronous methods. 
    /// </summary>
    public interface IMealsHttpRepository
    {
        /// <summary>
        /// GetMeals() which returns a UserMealDto object
        /// </summary>
        /// <returns>UserDto UserMeals</returns>
        Task<UserDto> GetMeals();
        /// <summary>
        /// UpdateMeals() which takes UserMealDto userMealDto as a parameter and returns no value
        /// </summary>
        /// <param name="userMealDto"></param>
        /// <returns>UserDto Usermeals</returns>
        Task<bool> UpdateMeals(UserMealDto userMealDto);
        /// <summary>
        /// GetMealsByDate() which returns UserMealDto object based on the date specified
        /// </summary>
        /// <param name="date"></param>
        /// <returns>UserDto UserMeals</</returns>
        Task<UserDto> GetMealsByDate(string date);
        /// <summary>
        /// DeleteMeals() which takes userMealID as a parameter and deletes a meal matching that Id
        /// </summary>
        /// <param name="userMealId"></param>
        /// <returns>response true or false<returns>
        Task<bool> DeleteMeals(string userMealId);
        /// <summary>
        /// AddMeals() which takes UserMealDto userMealDto as an object and adds it to the list of meals for a user
        /// </summary>
        /// <param name="userMealDto"></param>
        /// <returns>response true or false</returns>
        Task<bool> AddMeals(UserMealDto userMealDto);
    }
}
