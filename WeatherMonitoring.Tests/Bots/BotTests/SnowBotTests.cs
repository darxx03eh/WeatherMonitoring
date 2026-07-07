using WeatherMonitoring.Bots;
using WeatherMonitoring.Configurations;
using WeatherMonitoring.Models;
using WeatherMonitoring.Tests.Enums;

namespace WeatherMonitoring.Tests.Bots.BotTests;

public class SnowBotTests : WeatherBotTestBase<SnowBot>
{
    protected override SnowBot CreateBot(BotConfigurations config) => new(config);
    protected override WeatherData CreateWeatherData(decimal triggerValue) => new() { Temperature = triggerValue };
    protected override string ActivationKeyword => "SnowBot activated!";
    protected override ThresholdDirection Direction => ThresholdDirection.ActivatesBelow;

    protected override BotConfigurations CreateConfig(bool enabled, decimal threshold, string message) =>
        new() { Enabled = enabled, TemperatureThreshold = threshold, HumidityThreshold = null, Message = message };
}