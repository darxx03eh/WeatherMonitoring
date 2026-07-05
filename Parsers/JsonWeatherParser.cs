using System.Text.Json;
using WeatherMonitoring.Abstractions;
using WeatherMonitoring.Models;

namespace WeatherMonitoring.Parsers;

public class JsonWeatherParser : IWeatherParser
{
    /// <summary>
    /// Deserialize JSON input into <see cref="WeatherData"/> object
    /// </summary>
    /// <param name="data">The JSON string to deserialize. Cannot be <see langword="null"/> or empty.</param>
    /// <returns><see cref="WeatherData"/> object after deserialize it, or <see langword="null"/> if the JSON literal is <c>null</c>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="data"/> is <see langword="null"/>.</exception>
    /// <exception cref="JsonException">Thrown when <paramref name="data"/> is not valid JSON or does not match the expected shape.</exception>
    public WeatherData? Parse(string? data)
    => JsonSerializer.Deserialize<WeatherData>(data, new JsonSerializerOptions()
    {
        PropertyNameCaseInsensitive = true
    });
}