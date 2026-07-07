using WeatherMonitoring.Bots;
using WeatherMonitoring.Configurations;
using WeatherMonitoring.Models;
using WeatherMonitoring.Tests.Enums;

namespace WeatherMonitoring.Tests.Bots.BotTests;

public class RainBotTests : WeatherBotTestBase<RainBot>
{
    protected override RainBot CreateBot(BotConfigurations config) => new(config);
    protected override WeatherData CreateWeatherData(decimal triggerValue) => new() { Humidity = triggerValue };
    protected override string ActivationKeyword => "RainBot activated!";
    protected override ThresholdDirection Direction => ThresholdDirection.ActivatesAbove;

    protected override BotConfigurations CreateConfig(bool enabled, decimal threshold, string message) =>
        new() { Enabled = enabled, HumidityThreshold = threshold, TemperatureThreshold = null, Message = message };
}