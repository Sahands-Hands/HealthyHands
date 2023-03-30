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
        public async Task<bool> AddWeight(UserWeightDto userWeightDto)
        {
            var response = await _httpClient.PutAsJsonAsync("weights/add", userWeightDto);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;


        }

        /// <summary>
        ///  Updates users weight
        /// </summary>
        /// <param name="userWeightDto"></param>
        /// <returns></returns>
        public async Task<bool> UpdateWeight(UserWeightDto userWeightDto)
        {

            var response = await _httpClient.PutAsJsonAsync("weights/update", userWeightDto);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;

        }
        /// <summary>
        /// Deletes user Weight
        /// </summary>
        /// <param name="userWeightDto"></param>
        /// <returns></returns>
        public async Task<bool> DeleteWeight(UserWeightDto UserWeightId)
        {

            var response = await _httpClient.PutAsJsonAsync("weights/delete/{userWeightId:string}", UserWeightId);
            if (!response.IsSuccessStatusCode)
            {
                return false;

            }
            return true;

        }

    }
}