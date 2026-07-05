using WeatherMonitoring.Abstractions;
using WeatherMonitoring.Configurations;
using WeatherMonitoring.Models;

namespace WeatherMonitoring.Bots;

public class RainBot(BotConfigurations config) : IWeatherObserver
{
    private readonly BotConfigurations _config = config;
    public void Update(WeatherData weather)
    {
        if(!_config.Enabled) return;
        if(weather.Humidity > _config.Threshold)
            Console.WriteLine($"""
                               RainBot activated!
                               RainBot: {_config.Message}
                               """);
    }
}