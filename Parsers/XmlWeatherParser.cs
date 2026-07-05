using System.Xml.Serialization;
using WeatherMonitoring.Abstractions;
using WeatherMonitoring.Models;

namespace WeatherMonitoring.Parsers;

public class XmlWeatherParser : IWeatherParser
{
    public WeatherData? Parse(string data)
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(WeatherData));
        using StringReader stringReader = new StringReader(data);
        return xmlSerializer.Deserialize(stringReader) as WeatherData;
    }
}