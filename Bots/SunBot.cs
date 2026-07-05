using WeatherMonitoring.Abstractions;
using WeatherMonitoring.Configurations;
using WeatherMonitoring.Models;

namespace WeatherMonitoring.Bots;

public class SunBot(BotConfigurations config) : IWeatherObserver
{
    private readonly BotConfigurations _config = config;
    /// <summary>
    /// Handles a weather data update. If the bot is enabled and the temperature exceeds the
    /// configured threshold, prints an activation message to the console.
    /// </summary>
    /// <param name="weather">The latest weather data received from the publisher. Cannot be <see langword="null"/>.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="weather"/> is <see langword="null"/>.</exception>
    public void Update(WeatherData? weather)
    {
        ArgumentNullException.ThrowIfNull(weather);
        if (!_config.Enabled) return;
        if(weather?.Temperature > _config.Threshold)
            Console.WriteLine($"""
                               SunBot activated!
                               SunBot: {_config.Message}
                               """);
    }
}