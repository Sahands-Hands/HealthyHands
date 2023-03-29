using HealthyHands.Shared.Models;

public interface IWeightHttpRepository
{
    Task<UserDto> GetWeights();

    Task<UserDto> GetWeightsByWeightDate();

    Task AddWeight(UserWeightDto userWeightDto);

    Task UpdateWeight(UserWeightDto userWeightDto);

    Task DeleteWeight(UserWeightDto userWeightDto);


}