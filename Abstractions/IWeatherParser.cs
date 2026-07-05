using WeatherMonitoring.Models;

namespace WeatherMonitoring.Abstractions;

public interface IWeatherParser
{
    WeatherData? Parse(string data);
}