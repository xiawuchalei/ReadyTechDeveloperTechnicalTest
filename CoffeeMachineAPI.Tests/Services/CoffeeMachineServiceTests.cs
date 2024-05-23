using Moq;
using Xunit;
using CoffeeMachineAPI.Services;
using CoffeeMachineAPI.Factories;
using CoffeeMachineAPI.Exceptions;

namespace CoffeeMachineAPI.Tests.Services
{
    public class CoffeeMachineServiceTests
    {
        private readonly ICoffeeMachineService _coffeeMachineService;
        private readonly Mock<IWeatherService> _mockWeatherService;
        private readonly Mock<CoffeeBrewingStrategyFactory> _mockStrategyFactory;

        public CoffeeMachineServiceTests()
        {
            _mockWeatherService = new Mock<IWeatherService>();
            _mockStrategyFactory = new Mock<CoffeeBrewingStrategyFactory>();
            
            // Initialize CoffeeMachineService with the mocked IWeatherService and mock strategy
            _coffeeMachineService = new CoffeeMachineService(_mockWeatherService.Object, _mockStrategyFactory.Object);
        }

        [Fact]
        public async Task BrewCoffee_ThrowsHttpStatusException_ForFutureDate()
        {
            // Arrange
            var futureDate = DateTime.Now.AddHours(1);
            _mockWeatherService.Setup(s => s.GetCurrentTemperature(35.0m, 139.0m)).ReturnsAsync(20);

            // Assert that the result is as expected
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _coffeeMachineService.BrewCoffee(35.0m, 139.0m, futureDate));
            Assert.Equal("Date cannot be in the future", exception.Message);
        }

        [Fact]
        public async Task BrewCoffee_Throws_HttpStatusException_When_RequestCount_Modulo_5_Is_Zero()
        {
            // Arrange
            _mockWeatherService.Setup(s => s.GetCurrentTemperature(35.0m, 139.0m)).ReturnsAsync(32);
            ICoffeeMachineService coffeeMachineService = new CoffeeMachineService(_mockWeatherService.Object, _mockStrategyFactory.Object);
            
            for (int i = 0; i < 4; i++)
            {
                await coffeeMachineService.BrewCoffee(35.0m, 139.0m, DateTime.Now);
            }

            // 503 for 5th request
            var exception = await Assert.ThrowsAsync<HttpStatusException>(() => coffeeMachineService.BrewCoffee(35.0m, 139.0m, DateTime.Now));
            Assert.Equal(503, exception.StatusCode);
            Assert.Equal("Service Unavailable - Coffee machine needs maintenance", exception.Message);

            // 200 for 6th request
            var result = await coffeeMachineService.BrewCoffee(35.0m, 139.0m, DateTime.Now);
            Assert.NotNull(result);
            Assert.Equal("Your refreshing iced coffee is ready", result.Message);
        }

        [Fact]
        public async Task BrewCoffee_Returns_HotCoffee_When_Temperature_Above_30()
        {
            // Arrange
            _mockWeatherService.Setup(s => s.GetCurrentTemperature(35.0m, 139.0m)).ReturnsAsync(31);

            // Act
            var result = await _coffeeMachineService.BrewCoffee(35.0m, 139.0m, DateTime.Now);

            // Assert
            Assert.Equal("Your refreshing iced coffee is ready", result.Message);
            // Ensure the IWeatherService was called with correct parameters
            _mockWeatherService.Verify(s => s.GetCurrentTemperature(35.0m, 139.0m), Times.Once());
        }

        [Fact]
        public async Task BrewCoffee_Returns_IcedCoffee_When_Temperature_Below_30()
        {
            // Arrange
            _mockWeatherService.Setup(s => s.GetCurrentTemperature(35.0m, 139.0m)).ReturnsAsync(20);

            // Act
            var result = await _coffeeMachineService.BrewCoffee(35.0m, 139.0m, DateTime.Now);

            // Assert
            Assert.Equal("Your piping hot coffee is ready", result.Message);
            // Ensure the IWeatherService was called with correct parameters
            _mockWeatherService.Verify(s => s.GetCurrentTemperature(35.0m, 139.0m), Times.Once());
        }

        [Fact]
        public async Task BrewCoffee_Returns_TeapotMessageAndStatus_On_AprilFoolsDay()
        {
            // Arrange
            var aprilFoolsDay = new DateTime(2024, 4, 1);
            _mockWeatherService.Setup(s => s.GetCurrentTemperature(35.0m, 139.0m)).ReturnsAsync(20);

            // Assert that the result is as expected
            var exception = await Assert.ThrowsAsync<HttpStatusException>(() => _coffeeMachineService.BrewCoffee(35.0m, 139.0m, aprilFoolsDay));
            Assert.Equal(418, exception.StatusCode);
            Assert.Equal("I'm a teapot", exception.Message);
        }

        [Fact]
        public async Task BrewCoffee_ThrowsHttpStatusException_On_AprilFoolsDay()
        {
            // Arrange
            var aprilFoolsDay = new DateTime(2024, 4, 1);
            _mockWeatherService.Setup(s => s.GetCurrentTemperature(35.0m, 139.0m)).ReturnsAsync(20);

            // Assert that the result is as expected
            var exception = await Assert.ThrowsAsync<HttpStatusException>(() => _coffeeMachineService.BrewCoffee(35.0m, 139.0m, aprilFoolsDay));
            Assert.Equal(418, exception.StatusCode);
            Assert.Equal("I'm a teapot", exception.Message);
        }

        [Fact]
        public async Task BrewCoffee_Throws_Exception_When_WeatherService_Fails()
        {
            // Arrange
            _mockWeatherService.Setup(s => s.GetCurrentTemperature(It.IsAny<decimal>(), It.IsAny<decimal>()))
                            .ThrowsAsync(new System.Exception("Weather service unavailable"));

            // Act & Assert
            await Assert.ThrowsAsync<System.Exception>(() => _coffeeMachineService.BrewCoffee(35.0m, 139.0m, DateTime.Now));
        }
    }
}
