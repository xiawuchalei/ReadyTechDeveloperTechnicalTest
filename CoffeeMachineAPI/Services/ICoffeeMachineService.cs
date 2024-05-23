using CoffeeMachineAPI.Models;

namespace CoffeeMachineAPI.Services
{
    public interface ICoffeeMachineService
    {
        public Task<BrewCoffeeResponse> BrewCoffee(decimal lat, decimal lon, DateTime date);
    }
}