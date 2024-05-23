using CoffeeMachineAPI.Services;
using Moq;
using Moq.Protected;
using System.Net;
using Xunit;

namespace CoffeeMachineAPI.Tests.Services
{
    public class WeatherServiceTests
    {
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly WeatherService _weatherService;
        private readonly string _apiUrl = $"https://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&appid={2}&units=metric";

        public WeatherServiceTests()
        {
            _handlerMock = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri("https://api.openweathermap.org/")
            };
            _weatherService = new WeatherService(httpClient);
        }

        [Theory]
        [InlineData(-91, 139.0)]
        [InlineData(91, 139.0)]
        [InlineData(35.0, -181)]
        [InlineData(35.0, 181)]
        public async Task GetCurrentTemperature_ThrowsArgumentOutOfRangeException_ForOutOfRangeInput(decimal lat, decimal lon)
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _weatherService.GetCurrentTemperature(lat, lon));
        }

        [Fact]
        public async Task GetCurrentTemperature_ReturnsTemperatureFromApiResponse()
        {
            // Arrange
            var fakeTemperature = 25.5m;
            var fakeResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"current\":{\"temp\":" + fakeTemperature + "}}"),
            };
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(fakeResponse);

            // Act
            var result = await _weatherService.GetCurrentTemperature(35.0m, 139.0m);

            // Assert
            Assert.Equal(fakeTemperature, result + 273.15m);
        }

        [Fact]
        public async Task GetCurrentTemperature_ThrowsException_WhenApiFails()
        {
            // Arrange
            var fakeResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(fakeResponse);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => _weatherService.GetCurrentTemperature(35.0m, 139.0m));
        }
    }
}
