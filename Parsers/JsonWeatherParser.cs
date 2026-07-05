using System.Text.Json;
using WeatherMonitoring.Abstractions;
using WeatherMonitoring.Models;

namespace WeatherMonitoring.Parsers;

public class JsonWeatherParser : IWeatherParser
{
    public WeatherData? Parse(string data)
    => JsonSerializer.Deserialize<WeatherData>(data, new JsonSerializerOptions()
    {
        PropertyNameCaseInsensitive = true
    });
}