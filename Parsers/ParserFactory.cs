using System.Text.Json;
using System.Xml;
using WeatherMonitoring.Abstractions;
using OneOf;

namespace WeatherMonitoring.Parsers;

public static class ParserFactory
{
    public static readonly List<(Func<string, bool> match, IWeatherParser weatherParser)> Parse
        = new()
        {
            (input => input?[0] is '{' or '[' && IsValidJson(input), new JsonWeatherParser()),
            (input => input?[0] is '<' && IsValidXml(input), new XmlWeatherParser())
        };
    /// <summary>
    /// Determines the appropriate <see cref="IWeatherParser"/> for the given input based on its format.
    /// Returns a <see cref="JsonWeatherParser"/> if the input is valid JSON,
    /// or a <see cref="XmlWeatherParser"/> if the input is valid XML.
    /// </summary>
    /// <param name="data">The raw input string to detect and parse. May be</param>
    /// <returns>
    /// <see cref="IWeatherParser"/> if the input is valid and in a supported format;
    /// <see cref="ArgumentException"/> if the input is null, empty, or whitespace;
    /// <see cref="NotSupportedException"/> if the input does not match any supported format.
    /// </returns>
    public static OneOf<IWeatherParser, ArgumentException, NotSupportedException> GetParser(string? data)
    {
        data = data?.Trim();
        if (string.IsNullOrWhiteSpace(data) || data.Length == 0)
            return new ArgumentException($"Unknown format.");

        if (data.Length > 0 && data[0] == '\uFEFF')
            data = data?.Substring(1);

        var (_, weatherParser) = Parse.FirstOrDefault(entry => entry.match(data));
        return weatherParser is not null
            ? OneOf<IWeatherParser, ArgumentException, NotSupportedException>.FromT0(weatherParser)
            : new NotSupportedException("This format is not supported.");
    }
    /// <summary>
    /// Verifies if the input is formatted as JSON Format
    /// </summary>
    /// <param name="data">The string content to validate as JSON.</param>
    /// <returns>true if JSON is valid otherwise false</returns>
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
    /// <summary>
    /// Verifies if the input is formatted as XML Format
    /// </summary>
    /// <param name="data">The string content to validate as XML.</param>
    /// <returns>true if XML is valid otherwise false</returns>
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