using HealthyHands.Shared.Models;

namespace HealthyHands.Client.HttpRepository.WeightHttpRepository
{
    public interface IWeightHttpRepository
    {
        Task<UserDto> GetWeights();

        Task<UserDto> GetWeightsByWeightDate();

        Task<bool> AddWeight(UserWeightDto userWeightDto);

        Task<bool> UpdateWeight(UserWeightDto userWeightDto);

        Task<bool> DeleteWeight(string userWeightDto);


    }
}