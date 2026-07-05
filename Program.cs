using WeatherMonitoring.Abstractions;
using WeatherMonitoring.Bots;
using WeatherMonitoring.Models;
using WeatherMonitoring.Parsers;
using WeatherMonitoring.Services;

namespace WeatherMonitoring;

class Program
{
    static void Main(string[] args)
    {
        var config = ConfigurationsLoader.LoadConfigurations();
        IWeatherStation weatherStation = new WeatherStation();
        List<IWeatherObserver> observers = new()
        {
            new RainBot(config?.RainBot!),
            new SnowBot(config?.SnowBot!),
            new SunBot(config?.SunBot!)
        };
        observers.ForEach(weatherStation.Register);
        WeatherData? weatherData = null;
        Console.Write($"Enter weather data: ");
        string? input = Console.ReadLine();
        ParserFactory.GetParser(input).Switch(parser =>
            {
                weatherData = parser.Parse(input);
            },
            argumentException => Console.Error.WriteLine($"Argument Error: {argumentException.Message}"),
            notSupportedException => Console.Error.Write($"Not Supported Error: {notSupportedException.Message}")
            );
        weatherStation.Publish(weatherData);
    }
}