using Newtonsoft.Json;

namespace CoffeeMachineAPI.Services
{
    public class WeatherService : IWeatherService
{
    private readonly HttpClient httpClient;
    private const string ApiKey = "6c8a0597fb37a6d886b23cb3779e2794";

    public WeatherService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<decimal> GetCurrentTemperature(decimal lat, decimal lon)
    {
        ValidateCoordinates(lat, lon);

        var url = $"https://api.openweathermap.org/data/3.0/onecall?lat={lat}&lon={lon}&appid={ApiKey}";
        var response = await httpClient.GetStringAsync(url);
        var weatherData = JsonConvert.DeserializeObject<dynamic>(response);
        decimal tempKelvin = (decimal)weatherData.current.temp;
        decimal temp = tempKelvin - 273.15m;
        return temp;
    }

    private void ValidateCoordinates(decimal latitude, decimal longitude)
    {
        if (latitude < -90 || latitude > 90)
        {
            throw new ArgumentOutOfRangeException("Latitude must be between -90 and 90 degrees.");
        }

        if (longitude < -180 || longitude > 180)
        {
            throw new ArgumentOutOfRangeException("Longitude must be between -180 and 180 degrees.");
        }
    }
}
}