using FluentAssertions;
using WeatherMonitoring.Configurations;

namespace WeatherMonitoring.Tests.Bots;

public class BotConfigurationsTests
{
    [Fact]
    public void Threshold_ShouldReturnTemperatureThreshold_WhenOnlyTemperatureThresholdSet()
    {
        var config = new BotConfigurations
        {
            TemperatureThreshold = 25.5m, 
            HumidityThreshold = null
        };

        config.Threshold.Should().Be(25.5m);
    }

    [Fact]
    public void Threshold_ShouldReturnHumidityThreshold_WhenOnlyHumidityThresholdSet()
    {
        var config = new BotConfigurations
        {
            TemperatureThreshold = null, 
            HumidityThreshold = 60m
        };

        config.Threshold.Should().Be(60m);
    }

    [Fact]
    public void Threshold_ShouldReturnsZero_WhenNeitherThresholdsSet()
    {
        var config = new BotConfigurations
        {
            TemperatureThreshold = null, 
            HumidityThreshold = null
        };

        config.Threshold.Should().Be(0.0m);
    }
    [Fact]
    public void Threshold_ShouldPrefersTemperatureThresholdOverHumidity_WhenBotSet()
    {
        var config = new BotConfigurations
        {
            TemperatureThreshold = 30m, 
            HumidityThreshold = 70m
        };

        config.Threshold.Should().Be(30m);
    }
}