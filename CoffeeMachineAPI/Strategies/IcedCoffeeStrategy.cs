namespace CoffeeMachineAPI.Strategies
{
    public class IcedCoffeeStrategy : ICoffeeBrewingStrategy
    {
        public string Brew()
        {
            return "Your refreshing iced coffee is ready";
        }
    }
}