using CoffeeMachineAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CoffeeMachineAPI.Exceptions
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ArgumentException || context.Exception is ArgumentOutOfRangeException)
            {
                context.Result = new BadRequestObjectResult(new BrewCoffeeResponse
                {
                    Message = context.Exception.Message
                });
            }
            else if (context.Exception is HttpStatusException httpStatusException) 
            {
                context.Result = new ObjectResult(new BrewCoffeeResponse
                {
                    Message = httpStatusException.Message
                })
                {
                    StatusCode = httpStatusException.StatusCode
                };
            }
            else
            {
                context.Result = new ObjectResult(new BrewCoffeeResponse
                {
                    Message = "Internal Server Error"
                })
                {
                    StatusCode = 500
                };
            }
            context.ExceptionHandled = true;
        }
    }
}
