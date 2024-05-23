using CoffeeMachineAPI.Exceptions;
using CoffeeMachineAPI.Factories;
using CoffeeMachineAPI.Models;

namespace CoffeeMachineAPI.Services
{
    public class CoffeeMachineService: ICoffeeMachineService
    {
        private static int requestCount = 0;
        private readonly IWeatherService weatherService;
        private readonly CoffeeBrewingStrategyFactory strategyFactory;

        public CoffeeMachineService(IWeatherService weatherService,
                                    CoffeeBrewingStrategyFactory strategyFactory)
        {
            this.weatherService = weatherService;
            this.strategyFactory = strategyFactory;
            requestCount = 0;
        }

        async Task<BrewCoffeeResponse> ICoffeeMachineService.BrewCoffee(decimal lat, decimal lon, DateTime date)
        {
            if (date > DateTime.Now)
            {
                throw new ArgumentException("Date cannot be in the future");
            }

            requestCount++;

            if (date.Month == 4 && date.Day == 1)
            {
                throw new HttpStatusException(418, "I'm a teapot");
            }

            if (requestCount % 5 == 0)
            {
                throw new HttpStatusException(503, "Service Unavailable - Coffee machine needs maintenance");
            }

            decimal temperature = await weatherService.GetCurrentTemperature(lat, lon);
            var strategy = strategyFactory.GetBrewingStrategy(temperature);
            var offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now);
            var offsetString = (offset > TimeSpan.Zero ? "+" : "-") + offset.ToString("hhmm");
            return new BrewCoffeeResponse { 
                Message = strategy.Brew(), 
                Prepared = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss") + offsetString
                };
        }
    }
}
