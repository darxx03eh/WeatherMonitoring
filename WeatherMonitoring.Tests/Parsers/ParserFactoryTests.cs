using FluentAssertions;
using WeatherMonitoring.Abstractions;
using WeatherMonitoring.Parsers;
using WeatherMonitoring.Tests.Attributes;
using WeatherMonitoring.Tests.TestModels;
using WeatherMonitoring.Tests.TestModels.Parsers;

namespace WeatherMonitoring.Tests.Parsers;

public sealed class ParserFactoryTests
{
    [Theory]
    [NullOrWhiteSpaceData]
    public void Initialize_ShouldReturnArgumentException_WhenInputIsNullOrWhiteSpace(string? input)
    {
        var result = ParserFactory.GetParser(input);
        
        result.IsT0.Should().BeFalse();
        result.IsT1.Should().BeTrue();
        result.IsT2.Should().BeFalse();
        result.AsT1.Should().BeOfType<ArgumentException>();
        result.AsT1.Message.Should().Be("Unknown format.");
    }

    [Theory]
    [ObjectTypeData(typeof(InputTestCaseRow), "Parsers", "ParserFactory", "invalid-input-test-data.csv")]
    public void Initialize_ShouldReturnNotSupportedException_WhenInputIsInvalidOrNotSupported(InputTestCaseRow testCase)
    {
        var result = ParserFactory.GetParser(testCase.Input);
        
        result.IsT0.Should().BeFalse();
        result.IsT1.Should().BeFalse();
        result.IsT2.Should().BeTrue();
        result.AsT2.Should().BeOfType<NotSupportedException>();
        result.AsT2.Message.Should().Be("This format is not supported.");
    }

    [Theory]
    [ObjectTypeData(typeof(InputTestCaseRow), "Parsers", "ParserFactory", "valid-json-test-data.csv")]
    public void Initialize_ShouldReturnJsonWeatherParser_WhenInputIsJsonFormat(InputTestCaseRow testCase)
    {
        var result =  ParserFactory.GetParser(testCase.Input);

        result.IsT0.Should().BeTrue();
        result.IsT1.Should().BeFalse();
        result.IsT2.Should().BeFalse();
        result.AsT0.Should().BeOfType<JsonWeatherParser>();
        result.AsT0.Should().BeAssignableTo<IWeatherParser>();
    }
    [Theory]
    [ObjectTypeData(typeof(InputTestCaseRow),  "Parsers", "ParserFactory", "valid-xml-test-data.csv")]
    public void Initialize_ShouldReturnXmlWeatherParser_WhenInputIsXmlFormat(InputTestCaseRow testCase)
    {
        var result =  ParserFactory.GetParser(testCase.Input);

        result.IsT0.Should().BeTrue();
        result.IsT1.Should().BeFalse();
        result.IsT2.Should().BeFalse();
        result.AsT0.Should().BeOfType<XmlWeatherParser>();
        result.AsT0.Should().BeAssignableTo<IWeatherParser>();
    }
}