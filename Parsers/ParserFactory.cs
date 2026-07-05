using System.Text.Json;
using System.Xml;
using WeatherMonitoring.Abstractions;
using OneOf;

namespace WeatherMonitoring.Parsers;

public static class ParserFactory
{
    public static OneOf<IWeatherParser, ArgumentException, NotSupportedException> GetParser(string? data)
    {
        data = data.Trim();
        if (string.IsNullOrWhiteSpace(data) || data.Length == 0)
            return new ArgumentException($"Unknown format.");

        if (data.Length > 0 && data[0] == '\uFEFF')
            data = data.Substring(1);

        if (data[0] is '{' or '[' && IsValidJson(data))
            return new JsonWeatherParser();
        else if (data[0] is '<' && IsValidXml(data))
            return new XmlWeatherParser();
        else return new NotSupportedException("This format is not supported.");
    }

    private static bool IsValidJson(string data)
    {
        try
        {
            using JsonDocument _ =  JsonDocument.Parse(data);
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }

    private static bool IsValidXml(string data)
    {
        try
        {
            var doc = new XmlDocument();
            doc.LoadXml(data);
            return true;
        }
        catch (XmlException)
        {
            return false;
        }
    }
}