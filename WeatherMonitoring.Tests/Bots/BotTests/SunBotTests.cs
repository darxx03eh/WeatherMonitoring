using WeatherMonitoring.Bots;
using WeatherMonitoring.Configurations;
using WeatherMonitoring.Models;
using WeatherMonitoring.Tests.Enums;

namespace WeatherMonitoring.Tests.Bots.BotTests;

public class SunBotTests : WeatherBotTestBase<SunBot>
{
    protected override SunBot CreateBot(BotConfigurations config) => new(config);
    protected override WeatherData CreateWeatherData(decimal triggerValue) => new() { Temperature = triggerValue };
    protected override string ActivationKeyword => "SunBot activated!";
    protected override ThresholdDirection Direction => ThresholdDirection.ActivatesAbove;

    protected override BotConfigurations CreateConfig(bool enabled, decimal threshold, string message) =>
        new() { Enabled = enabled, TemperatureThreshold = threshold, HumidityThreshold = null, Message = message };
}