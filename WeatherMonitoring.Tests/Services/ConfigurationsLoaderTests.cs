using System.Text.Json;
using FluentAssertions;
using WeatherMonitoring.Configurations;
using WeatherMonitoring.Services;
using WeatherMonitoring.Tests.Attributes;
using WeatherMonitoring.Tests.TestModels;
using WeatherMonitoring.Tests.TestModels.Services;

namespace WeatherMonitoring.Tests.Services;

public sealed class ConfigurationsLoaderTests
{
    [Theory]
    [ObjectTypeData(typeof(ConfigTestCaseRow), "TestData", "Services", "ConfigurationsLoader", "config-test-data.csv")]
    public void Load_ShouldReturnConfiguration_WhenJsonIsValid(ConfigTestCaseRow testCase)
    {
        var result = ConfigurationsLoader.Deserialize(testCase.Input);

        ApplicationConfigurations expected = new()
        {
            RainBot = new BotConfigurations
            {
                Enabled = testCase.RainBotEnabled,
                HumidityThreshold = testCase.RainBotThreshold,
                Message = testCase.RainBotMessage
            },
            SunBot = new BotConfigurations
            {
                Enabled = testCase.SunBotEnabled,
                TemperatureThreshold = testCase.SunBotThreshold,
                Message = testCase.SunBotMessage
            },
            SnowBot = new BotConfigurations
            {
                Enabled = testCase.SnowBotEnabled,
                TemperatureThreshold = testCase.SnowBotThreshold,
                Message = testCase.SnowBotMessage
            }
        };

        result.Should().NotBeNull();
        result.Should().BeOfType<ApplicationConfigurations>();
        result.Should().BeEquivalentTo(expected);
    }

    [Theory]
    [ObjectTypeData(typeof(InputTestCaseRow), "TestData","Services", "ConfigurationsLoader", "missing-file-test-data.csv")]
    public void Load_ShouldThrowFileNotFoundException_WhenFileDoesNotExist(InputTestCaseRow testCase)
    {
        Action act = () => ConfigurationsLoader.LoadConfigurations(testCase.Input);

        act.Should().Throw<FileNotFoundException>()
            .WithMessage($"Configuration file not found at: {testCase.Input}");
    }
    [Theory]
    [ObjectTypeData(typeof(InputTestCaseRow), "TestData","Services", "ConfigurationsLoader", "invalid-json-test-data.csv")]
    public void Load_ShouldThrowJsonException_WhenJsonIsInvalid(InputTestCaseRow testCase)
    {
        Action act = () => ConfigurationsLoader.Deserialize(testCase.Input);

        act.Should().Throw<JsonException>();
    }
}