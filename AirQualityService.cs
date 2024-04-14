using System;

public class AirQualityService
{
    private readonly Random _random = new Random();

    public AirQualityIndex GetAirQuality(string city)
    {
        try
        {
            int aqi = _random.Next(0, 501);
            var description = GetDescription(aqi);
            return new AirQualityIndex(city, aqi, description);
        }
        catch (Exception e)
        {
            throw new Exception($"An error occurred: {e.Message}");
        }
    }

    private string GetDescription(int aqi)
    {
        if (aqi <= 50) return "Good";
        if (aqi <= 100) return "Moderate";
        if (aqi <= 150) return "Unhealthy for Sensitive Groups";
        if (aqi <= 200) return "Unhealthy";
        if (aqi <= 300) return "Very Unhealthy";
        return "Hazardous";
    }
}

public record AirQualityIndex(string City, int AQI, string Description);