using Hogwarts.Middleware.Dtos;
using Hogwarts.Middleware.Interfaces.Service;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hogwarts.Middleware.Services
{
    public class CharacterService : ICharacterService
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;

        public CharacterService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IMemoryCache memoryCache)
        {

            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _memoryCache = memoryCache;
        }

        public async Task<bool> Delete(Guid id)
        {

            var request = id.ToString();
            var client = _httpClientFactory.CreateClient("HogwartsRepositoryApiService");
            var result = await client.DeleteAsync(request);


            if (result.IsSuccessStatusCode)
            {
                var responseContent = result.Content.ReadAsStringAsync().Result;
                return Convert.ToBoolean(responseContent);
            }
            else
            {
                return false;
            }
        }

        public async Task<CharacterResultDto> Get(Guid id)
        {
            var request = id.ToString();
            var client = _httpClientFactory.CreateClient("HogwartsRepositoryApiService");
            var result = await client.GetAsync(request);


            if (result.IsSuccessStatusCode)
            {
                var responseContent = result.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<CharacterResultDto>(responseContent);

                return obj;
            }
            return null;
        }

        public async Task<IEnumerable<CharacterResultDto>> GetAllCharacterHouse(string house)
        {
            var request = $"GetAllCharacterHouse/house={house}";
            var client = _httpClientFactory.CreateClient("HogwartsRepositoryApiService");
            var result = await client.GetAsync(request);


            if (result.IsSuccessStatusCode)
            {
                var responseContent = result.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<IEnumerable<CharacterResultDto>>(responseContent);

                return obj;
            }
            return null;
        }

        public async Task<CharacterResultDto> Post(CharacterInsertDto character)
        {

            var jsonContent = JsonConvert.SerializeObject(character);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient("HogwartsRepositoryApiService");
            var result = await client.PostAsync("", contentString);
            if (result.IsSuccessStatusCode)
            {
                var responseContent = result.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<CharacterResultDto>(responseContent);

                return obj;
            }
            return null;
        }

        public async Task<CharacterResultDto> Put(CharacterUpdateDto character)
        {
            var jsonContent = JsonConvert.SerializeObject(character);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient("HogwartsRepositoryApiService");
            var result = await client.PutAsync("", contentString);
            if (result.IsSuccessStatusCode)
            {
                var responseContent = result.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<CharacterResultDto>(responseContent);

                return obj;
            }
            return null;
        }


        public async Task<bool> GetHousesApiPotter(Guid id)
        {
            var cacheKey = "PotterApiService";
            if (!_memoryCache.TryGetValue(cacheKey, out Houses obj))
            {

                var apikey = _configuration.GetValue(typeof(string), "apikey");
                var client = _httpClientFactory.CreateClient("PotterApiService");
                client.DefaultRequestHeaders.Add("apikey", apikey.ToString());
                var request = $"houses";
                var result = await client.GetAsync(request);

                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(5),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromMinutes(2)
                };
                
                if (result.IsSuccessStatusCode)
                {
                    var responseContent = result.Content.ReadAsStringAsync().Result;
                    obj = JsonConvert.DeserializeObject<Houses>(responseContent);
                }
                else
                    return false;

                _memoryCache.Set(cacheKey, obj, cacheExpiryOptions);
            }

            var exists = await ValidHouseExists(obj, id);
            return exists;
        }


        public async Task<bool> ValidHouseExists(Houses houses, Guid idHouse)
        {
            var exists = houses.HousesHouses.Where(T => T.Id == idHouse).Count() > 0;
            return exists;
        }

    }
}

