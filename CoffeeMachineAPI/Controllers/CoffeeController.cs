using Microsoft.AspNetCore.Mvc;
using CoffeeMachineAPI.Services;
using System.Threading.Tasks;

namespace CoffeeMachineAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CoffeeController : ControllerBase
    {
        private readonly CoffeeMachineService coffeeMachineService;

        public CoffeeController(CoffeeMachineService coffeeMachineService)
        {
            this.coffeeMachineService = coffeeMachineService;
        }

        [HttpGet("brew-coffee")]
        public async Task<IActionResult> BrewCoffee()
        {
            var (statusCode, responseContent) = await coffeeMachineService.BrewCoffee();
            
            return statusCode switch
            {
                200 => Ok(responseContent),
                503 => StatusCode(503),
                418 => StatusCode(418),
                _ => BadRequest("Invalid request.")
            };
        }
    }
}
