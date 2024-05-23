namespace CoffeeMachineAPI.Services
{
    public interface IWeatherService
    {
        Task<decimal> GetCurrentTemperature(decimal lat, decimal lon);
    }
}