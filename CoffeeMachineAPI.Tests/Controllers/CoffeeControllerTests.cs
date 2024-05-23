using Xunit;
using Moq; 
using Microsoft.AspNetCore.Mvc;
using CoffeeMachineAPI.Controllers;
using CoffeeMachineAPI.Services;
using CoffeeMachineAPI.Models;

namespace CoffeeMachineAPI.Tests.Controllers
{
    public class CoffeeControllerTests
    {
        [Fact]
        public async Task BrewCoffee_ReturnsHotCoffee_WhenTemperatureIsLow()
        {
            // Arrange
            var coffeeServiceMock = new Mock<ICoffeeMachineService>();
            coffeeServiceMock.Setup(service => service.BrewCoffee(It.IsAny<decimal>(), 
                                                                    It.IsAny<decimal>(), 
                                                                    It.IsAny<DateTime>())).ReturnsAsync(new BrewCoffeeResponse { Message = "Your piping hot coffee is ready" });

            var controller = new CoffeeController(coffeeServiceMock.Object);

            // Act
            var result = await controller.BrewCoffee(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<DateTime>());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);        
            var response = Assert.IsType<BrewCoffeeResponse>(okResult.Value);  
            Assert.Equal("Your piping hot coffee is ready", response.Message);         
        }

        [Fact]
        public async Task PostBrewCoffee_ReturnsIcedCoffee_WhenTemperatureIsHigh()
        {
            // Arrange
            var coffeeServiceMock = new Mock<ICoffeeMachineService>();
            coffeeServiceMock.Setup(service => service.BrewCoffee(It.IsAny<decimal>(), 
                                                                    It.IsAny<decimal>(), 
                                                                    It.IsAny<DateTime>())).ReturnsAsync(new BrewCoffeeResponse { Message = "Your refreshing iced coffee is ready" });

            var controller = new CoffeeController(coffeeServiceMock.Object);

            // Act
            var result = await controller.BrewCoffee(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<DateTime>());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);        
            var response = Assert.IsType<BrewCoffeeResponse>(okResult.Value);  
            Assert.Equal("Your refreshing iced coffee is ready", response.Message);  
        }
    }
}
