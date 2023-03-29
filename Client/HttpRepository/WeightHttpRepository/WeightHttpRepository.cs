using HealthyHands.Shared.Models;
using System.Net.Http.Json;

namespace HealthyHands.Client.HttpRepository.WeightHttpRepository
{
    public class WeightHttpRepository : IWeightHttpRepository
    {
        private readonly HttpClient _httpClient;
        public WeightHttpRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        /// <summary>
        ///  Gets the user weights.
        /// </summary>
        /// <returns></returns>
        public async Task<UserDto> GetWeights() 
        {
            UserDto user = await _httpClient.GetFromJsonAsync<UserDto>("weights"); 
            return user;
        }

        /// <summary>
        ///  Gets the user weights by date.
        /// </summary>
        /// <returns></returns>
        public async Task<UserDto> GetWeightsByWeightDate() 
        {
            UserDto user = await _httpClient.GetFromJsonAsync<UserDto>("weights/byDate/{date:string}");
            return user;

        }



        /// <summary>
        ///  Adds Users weight
        /// </summary>
        /// <param name="userWeightDto"></param>
        /// <returns></returns>
        public async Task AddWeight(UserWeightDto userWeightDto) // rename to AddWeight()
        {
            var response = await _httpClient.PutAsJsonAsync("weights/add", userWeightDto); // route is "weights/add"
           
        }

        /// <summary>
        ///  Updates users weight
        /// </summary>
        /// <param name="userWeightDto"></param>
        /// <returns></returns>
        public async Task UpdateWeight(UserWeightDto userWeightDto)
        {

            var response = await _httpClient.PutAsJsonAsync("weights/update", userWeightDto); // route is "weights/update"
            
        }
        /// <summary>
        /// Deletes user Weight
        /// </summary>
        /// <param name="userWeightDto"></param>
        /// <returns></returns>
        public async Task DeleteWeight(UserWeightDto UserWeightId) 
        {

            var response = await _httpClient.PutAsJsonAsync("weights/delete/{userWeightId:string}", UserWeightId); 

        }

    }
}