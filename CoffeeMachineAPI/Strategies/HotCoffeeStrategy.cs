namespace CoffeeMachineAPI.Strategies
{
    public class HotCoffeeStrategy : ICoffeeBrewingStrategy
    {
        public string Brew()
        {
            return "Your piping hot coffee is ready";
        }
    }
}