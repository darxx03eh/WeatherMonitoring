using FluentAssertions;
using WeatherMonitoring.Abstractions;
using WeatherMonitoring.Models;
using WeatherMonitoring.Parsers;
using WeatherMonitoring.Tests.Attributes.ParserAttributes;
using WeatherMonitoring.Tests.TestModels.Parsers;

namespace WeatherMonitoring.Tests.Parsers;

public sealed class XmlWeatherParserTests
{
    private readonly IWeatherParser _weatherParser;
    public XmlWeatherParserTests() => _weatherParser = new XmlWeatherParser();

    [Theory]
    [ValidXmlData]
    public void Parse_ShouldReturnValidWeatherData_WhenXmlIsValid(WeatherDataTestCaseRow testCase)
    {
        var result = _weatherParser.Parse(testCase.Input);
        
        WeatherData expected = new()
        {
            Location = testCase.ExpectedLocation,
            Temperature = testCase.ExpectedTemperature,
            Humidity = testCase.ExpectedHumidity,
        };
        
        result.Should().NotBeNull();
        result.Should().BeOfType<WeatherData>();
        result.Should().BeEquivalentTo(expected);
    }

    [Theory]
    [InValidXmlData]
    public void Parse_ShouldThrowInvalidOperationException_WhenXmlIsNotValid(string xml)
    {
        Action act = () => _weatherParser.Parse(xml);
        
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Parse_ShouldThrowArgumentNullException_WhenInputIsNull()
    {
        Action act = () => _weatherParser.Parse(null);
        
        act.Should().Throw<ArgumentNullException>();
    }
}