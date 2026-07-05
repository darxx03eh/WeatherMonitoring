using WeatherMonitoring.Abstractions;
using WeatherMonitoring.Configurations;
using WeatherMonitoring.Models;

namespace WeatherMonitoring.Bots;

public class SunBot(BotConfigurations config) : IWeatherObserver
{
    private readonly BotConfigurations _config = config;
    public void Update(WeatherData weather)
    {
        if (!_config.Enabled) return;
        if(weather.Temperature > _config.Threshold)
            Console.WriteLine($"""
                               SunBot activated!
                               SunBot: {_config.Message}
                               """);
    }
}