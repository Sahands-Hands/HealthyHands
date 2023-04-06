using HealthyHands.Shared.Models;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

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
            var responseMessage = await _httpClient.GetAsync("weights");
            var content = await ValidateContent(responseMessage).ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<UserDto>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return user;
        }

        /// <summary>
        ///  Gets the user weights by date.
        /// </summary>
        /// <returns></returns>
        public async Task<UserDto> GetWeightsByWeightDate()
        {
            var user = await _httpClient.GetFromJsonAsync<UserDto>("weights/byDate/{date:string}");
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
        public async Task<bool> DeleteWeight(string userWeightId)
        {

            var response = await _httpClient.DeleteAsync($"weights/delete/{userWeightId}");
            return response.IsSuccessStatusCode;

        }
        
        private HttpContent ValidateContent(HttpResponseMessage response)
        {
            if(string.IsNullOrEmpty(response.Content?.ReadAsStringAsync().Result))
            {
                return response.Content= new StringContent("null",Encoding.UTF8, MediaTypeNames.Application.Json);
            }
            else
            {
                return response.Content;
            }
        }

    }
}