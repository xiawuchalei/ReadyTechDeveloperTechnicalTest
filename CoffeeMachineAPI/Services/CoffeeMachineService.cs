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

            // Check if the date is April 1st, then all calls should return 418
            if (DateTime.Today.Month == 4 && DateTime.Today.Day == 1)
            {
                return (418, "");
            }

            // Check if the coffee machine needs to be refilled every fifth request
            if (requestCount % 5 == 0)
            {
                return (503, "");
            }

            string message = "Your piping hot coffee is ready";
            string responseContent = $"{{ \"message\": \"{message}\", \"prepared\": \"{DateTime.UtcNow:O}\" }}";

            return (200, responseContent);
        }
    }
}
