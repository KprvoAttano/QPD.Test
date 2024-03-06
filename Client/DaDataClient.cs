using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Client
{
    public class DaDataClient
    {
        public HttpClient Client { get; set; }

        public DaDataClient(HttpClient client, IConfiguration configuration)
        {
            Client = client;

            Client.BaseAddress = new Uri("https://cleaner.dadata.ru/api/v1/clean/address/");
            client.DefaultRequestHeaders.Clear();

            client.DefaultRequestHeaders.Add("Application-type", "application/json");
            client.DefaultRequestHeaders.Add("Authorization", "Token " + configuration["DaDataHeaders:token"]);
            client.DefaultRequestHeaders.Add("X-Secret", configuration["DaDataHeaders:secret"]);
        }
    }
}
