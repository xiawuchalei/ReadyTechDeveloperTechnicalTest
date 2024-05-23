using Microsoft.AspNetCore.Mvc;
using CoffeeMachineAPI.Services;
using CoffeeMachineAPI.Exceptions;

namespace CoffeeMachineAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ServiceFilter(typeof(CustomExceptionFilter))]
    public class CoffeeController : ControllerBase
    {
        private readonly ICoffeeMachineService coffeeMachineService;

        public CoffeeController(ICoffeeMachineService coffeeMachineService)
        {
            this.coffeeMachineService = coffeeMachineService;
        }

        [HttpGet("brew-coffee")]
        public async Task<IActionResult> BrewCoffee([FromQuery] decimal lat, [FromQuery] decimal lon, [FromQuery] DateTime date)
        {
            var response = await coffeeMachineService.BrewCoffee(lat, lon, date);
            return Ok(response);
        }
    }
}
