using CoffeeMachineAPI.Exceptions;
using CoffeeMachineAPI.Factories;
using CoffeeMachineAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddScoped<CoffeeBrewingStrategyFactory>();
builder.Services.AddScoped<ICoffeeMachineService, CoffeeMachineService>();
builder.Services.AddScoped<IWeatherService, WeatherService>();

// Add exception filters to the container
builder.Services.AddScoped<CustomExceptionFilter>();
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new CustomExceptionFilter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
