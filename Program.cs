using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Configure Kestrel server
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5167);
});

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<AirQualityService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// weatherforecast
app.MapGet("/weatherforecast", (WeatherForecastService forecastService, ILogger<Program> logger) =>
{
    logger.LogInformation(new EventId(1), "Request received for /weatherforecast");
    return forecastService.GetForecast();
}).WithName("GetWeatherForecast").WithOpenApi();

// airquality/{city}
app.MapGet("/airquality/{city}", (AirQualityService aqService, ILogger<Program> logger, string city) =>
{
    logger.LogInformation(new EventId(2), "Request received for /airquality with city: {City}", city);
    try
    {
        var result = aqService.GetAirQuality(city.Trim());
        logger.LogInformation(new EventId(3), "Successfully retrieved air quality for {City}", city);
        return Results.Ok(result);
    }
    catch (Exception ex)
    {
        logger.LogError(new EventId(4), ex, "An error occurred while retrieving air quality for {City}", city);
        return Results.Problem("An error occurred: " + ex.Message);
    }
});


app.Run();