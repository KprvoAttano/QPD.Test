using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Dadata;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Microsoft.Net.Http.Headers;
using System.Net.Http;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Service.Services
{
    public class DaDataService : IDaDataService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<DaDataService> _logger;

        public DaDataService(IHttpClientFactory httpClientFactory, ILogger<DaDataService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<Address> GetJsonFromDaData(AddressDto address)
        {
            try
            {
                _logger.LogDebug("Inside client Task");

                var httpClient = _httpClientFactory.CreateClient("DaDataClient");

                var content = JsonConvert.SerializeObject(new[] { address.Address });
                _logger.LogInformation($"Created content: {content}");

                using (var response =
                       await httpClient.PostAsync("", new StringContent(content, Encoding.UTF8, "application/json")))
                {
                    response.EnsureSuccessStatusCode();

                    var resp = response.Content.ReadAsStringAsync().Result;

                    var dataObject = JsonConvert.DeserializeObject<List<Address>>(resp).First();

                    _logger.LogDebug($"Outside client Task with result: {dataObject.result}");
                    return dataObject;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[ERROR] DaDataService. Error: {ex.Message}");
                return null;
            }
        }
    }
}
