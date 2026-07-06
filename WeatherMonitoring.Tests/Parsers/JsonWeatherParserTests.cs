using System.Text.Json;
using FluentAssertions;
using WeatherMonitoring.Abstractions;
using WeatherMonitoring.Models;
using WeatherMonitoring.Parsers;
using WeatherMonitoring.Tests.Attributes;
using WeatherMonitoring.Tests.TestModels;
using WeatherMonitoring.Tests.TestModels.Parsers;
using WeatherMonitoring.Tests.TestModels.Services;

namespace WeatherMonitoring.Tests.Parsers;

public sealed class JsonWeatherParserTests
{
    private readonly IWeatherParser _weatherParser;
    public JsonWeatherParserTests() => _weatherParser = new JsonWeatherParser();
    [Theory]
    [ObjectTypeData(typeof(WeatherDataTestCaseRow), "Parsers", "JsonWeatherParser", "valid-json-test-data.csv")]
    public void Parse_ShouldReturnWeatherData_WhenJsonValid(WeatherDataTestCaseRow testCase)
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
    [ObjectTypeData(typeof(InputTestCaseRow), "Services", "ConfigurationsLoader", "invalid-json-test-data.csv")]
    public void Parse_ShouldThrowJsonException_WhenJsonInvalid(InputTestCaseRow testCase)
    {
        Action act = () => _weatherParser.Parse(testCase.Input);

        act.Should().Throw<JsonException>();
    }

    [Fact]
    public void Parse_ShouldThrowArgumentNullException_WhenInputIsNull()
    {
        Action act = () => _weatherParser.Parse(null);
        
        act.Should().Throw<ArgumentNullException>();
    }
}