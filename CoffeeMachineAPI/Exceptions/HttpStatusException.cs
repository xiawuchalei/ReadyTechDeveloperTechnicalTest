namespace CoffeeMachineAPI.Exceptions
{
    public class HttpStatusException : Exception
    {
        public int StatusCode { get; }
        public string Message { get; }

        public HttpStatusException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}