using FluentAssertions;
using WeatherMonitoring.Abstractions;
using WeatherMonitoring.Parsers;
using WeatherMonitoring.Tests.Attributes.ParserAttributes;

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
    [InValidInputData]
    public void Initialize_ShouldReturnNotSupportedException_WhenInputIsInvalidOrNotSupported(string? input)
    {
        var result = ParserFactory.GetParser(input);
        
        result.IsT0.Should().BeFalse();
        result.IsT1.Should().BeFalse();
        result.IsT2.Should().BeTrue();
        result.AsT2.Should().BeOfType<NotSupportedException>();
        result.AsT2.Message.Should().Be("This format is not supported.");
    }

    [Theory]
    [ValidJsonInputData]
    public void Initialize_ShouldReturnJsonWeatherParser_WhenInputIsJsonFormat(string? input)
    {
        var result =  ParserFactory.GetParser(input);

        result.IsT0.Should().BeTrue();
        result.IsT1.Should().BeFalse();
        result.IsT2.Should().BeFalse();
        result.AsT0.Should().BeOfType<JsonWeatherParser>();
        result.AsT0.Should().BeAssignableTo<IWeatherParser>();
    }
    [Theory]
    [ValidXmlInputData]
    public void Initialize_ShouldReturnXmlWeatherParser_WhenInputIsXmlFormat(string input)
    {
        var result =  ParserFactory.GetParser(input);

        result.IsT0.Should().BeTrue();
        result.IsT1.Should().BeFalse();
        result.IsT2.Should().BeFalse();
        result.AsT0.Should().BeOfType<XmlWeatherParser>();
        result.AsT0.Should().BeAssignableTo<IWeatherParser>();
    }
}