using IpBlockApi.Models;
using IpBlockApi.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;

namespace IpBlockApi.Repository
{
    public class GeoRepository : IGeoService
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;

        public GeoRepository(HttpClient httpClient , IConfiguration configuration)   // inject Httpclient to send request 
                                                                                      // && Iconfiguration to read from app setings 
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
        }
        public async Task<GeoLocationResponse> LookIPasync(string IpAdress)
        {
            var apikey = configuration["GeoLocationApi:ApiKey"];
            var baseurl = configuration["GeoLocationApi:BaseUrl"];
            var url = $"{baseurl}?apiKey={apikey}";
            if (!string.IsNullOrWhiteSpace(IpAdress))
            {
                url += $"&ip={IpAdress}";
            }

            var response = await httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("failed to get information ");
               
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<GeoLocationResponse>(content);

        }
    }
}
