using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CoffeeMachineAPI.Services
{
    public class CoffeeMachineService
    {
        private static int requestCount = 0;
        private readonly HttpClient httpClient;
        private const string ApiKey = "6c8a0597fb37a6d886b23cb3779e2794";
        private const string lat = "-37.787003";
        private const string lon = "175.279251";


        public CoffeeMachineService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<(int StatusCode, string ResponseContent)> BrewCoffee()
        {
            requestCount++;

            if (DateTime.Today.Month == 4 && DateTime.Today.Day == 1)
            {
                return (418, "");
            }

            if (requestCount % 5 == 0)
            {
                return (503, "");
            }

            // Check weather and decide on coffee type
            decimal temperature = await GetTemperature();
            string message = temperature > 30 ? "Your refreshing iced coffee is ready" : "Your piping hot coffee is ready";

            return (200, message);
        }

        private async Task<decimal> GetTemperature()
        {
            var url = $"https://api.openweathermap.org/data/3.0/onecall?lat={lat}&lon={lon}&appid={ApiKey}";
            var response = await httpClient.GetStringAsync(url);
            var weatherData = JsonConvert.DeserializeObject<dynamic>(response);
            decimal tempKelvin = (decimal)weatherData.current.temp;
            decimal temp = tempKelvin - 273.15m;
            return temp;
        }
    }
}
