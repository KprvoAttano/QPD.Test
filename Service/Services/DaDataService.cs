using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Dadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using Domain.Interfaces;
using Domain.JsonSerializeOptions;
using Domain.Models;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

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

        public async Task<List<Address>> GetJsonFromDaDataAsync(AddressDto address, CancellationToken token)
        {
            try
            {
                _logger.LogDebug("Inside client Task");

                var httpClient = _httpClientFactory.CreateClient("DaDataClient");

                var addArray = new[] { address.Address };

                var content = JsonSerializer.Serialize(addArray, AddressDtoJsonSerializeOptions.Options);
                _logger.LogInformation("Created content: {0}", content);

                using (var response =
                       await httpClient.PostAsync("", new StringContent(content, Encoding.UTF8, "application/json"), token))
                {
                    response.EnsureSuccessStatusCode();

                    var resp = await response.Content.ReadAsStreamAsync(token);

                    var data = ((List<Address>) await JsonSerializer.DeserializeAsync(resp, typeof(List<Address>)));

                    _logger.LogDebug("Outside client Task with clean address: {0}", data);
                    return data;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("[ERROR] DaDataService. Error: {0}", ex.Message);
                return null;
            }
        }
    }
}
