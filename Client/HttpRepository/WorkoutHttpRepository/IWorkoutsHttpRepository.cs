using HealthyHands.Shared.Models;

namespace HealthyHands.Client.HttpRepository.WorkoutsRepository
{
    public interface IWorkoutsHttpRepository
    {
        Task<UserDto> GetWorkouts();
        Task<bool> UpdateWorkouts(UserWorkoutDto userWorkoutDto);
        Task<bool> DeleteWorkout(string userWorkoutId);
        Task<UserDto> GetWorkoutsByDate(string date);
        Task<bool> AddUserWorkout(UserWorkoutDto userWorkoutDto);

    }
}
