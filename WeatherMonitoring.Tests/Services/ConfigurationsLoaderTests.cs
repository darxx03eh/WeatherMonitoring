using System.Xml;
using FluentAssertions;
using WeatherMonitoring.Configurations;
using WeatherMonitoring.Services;
using WeatherMonitoring.Tests.Attributes.ConfigurationAttributes;
using WeatherMonitoring.Tests.TestModels;

namespace WeatherMonitoring.Tests.Services;

public sealed class ConfigurationsLoaderTests
{
    [Theory]
    [ApplicationConfigurationsData]
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
    [MissingFileData]
    public void Load_ShouldThrowFileNotFoundException_WhenFileDoesNotExist(string path)
    {
        Action act = () => ConfigurationsLoader.LoadConfigurations(path);

        act.Should().Throw<FileNotFoundException>()
            .WithMessage($"Configuration file not found at: {path}");
    }
}