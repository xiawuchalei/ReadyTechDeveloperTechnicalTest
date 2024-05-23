using CoffeeMachineAPI.Strategies;

namespace CoffeeMachineAPI.Factories
{
    public class CoffeeBrewingStrategyFactory
    {
        public ICoffeeBrewingStrategy GetBrewingStrategy(decimal temperature)
        {
            if (temperature > 30)
            {
                return new IcedCoffeeStrategy();
            }
            else
            {
                return new HotCoffeeStrategy();
            }
        }
    }
}