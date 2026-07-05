using System.Xml.Serialization;
using WeatherMonitoring.Abstractions;
using WeatherMonitoring.Models;

namespace WeatherMonitoring.Parsers;

public class XmlWeatherParser : IWeatherParser
{
    /// <summary>
    /// Deserialize XML input into <see cref="WeatherData"/> object
    /// </summary>
    /// <param name="data">The XML string to deserialize. Cannot be <see langword="null"/> or empty.</param>
    /// <returns><see cref="WeatherData"/> object after deserialize it, or <see langword="null"/> if the result cannot be cast to <see cref="WeatherData"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="data"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">Thrown when <paramref name="data"/> is not well-formed XML or does not match the expected shape.</exception>
    public WeatherData? Parse(string? data)
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(WeatherData));
        using StringReader stringReader = new StringReader(data);
        return xmlSerializer.Deserialize(stringReader) as WeatherData;
    }
}