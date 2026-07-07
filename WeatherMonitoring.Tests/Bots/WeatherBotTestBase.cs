using FluentAssertions;
using WeatherMonitoring.Abstractions;
using WeatherMonitoring.Bots;
using WeatherMonitoring.Configurations;
using WeatherMonitoring.Models;
using WeatherMonitoring.Tests.Captures;
using WeatherMonitoring.Tests.Enums;

namespace WeatherMonitoring.Tests.Bots;

public abstract class WeatherBotTestBase<TBot> where TBot : IWeatherObserver
{
    protected abstract TBot CreateBot(BotConfigurations config);
    protected abstract WeatherData CreateWeatherData(decimal triggerValue);
    protected abstract string ActivationKeyword { get; }
    protected abstract ThresholdDirection Direction { get; }
    protected abstract BotConfigurations CreateConfig(bool enabled, decimal threshold, string message);

    [Fact]
    public void Update_ShouldThrowArgumentNullException_WhenWeatherDataIsNull()
    {
        var bot = CreateBot(CreateConfig(enabled: true, threshold: 50m, message: "msg"));

        Action act = () => bot.Update(null);

        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("weather");
    }

    [Fact]
    public void Update_ShouldNeverActivates_WhenDisabled()
    {
        var bot = CreateBot(CreateConfig(enabled: false, threshold: 50m, message: "msg"));
        var triggeringValue = Direction == ThresholdDirection.ActivatesAbove ? 1000m : -1000m;

        ConsoleCapture.Capture(writer =>
        {
            bot.Update(CreateWeatherData(triggeringValue));
            writer.ToString().Should().BeEmpty();
        });
    }

    [Fact]
    public void Update_ShouldActivatesOnlyWhenThresholdStrictlyCrossed_WhenEnabled()
    {
        const decimal threshold = 50m;
        var bot = CreateBot(CreateConfig(enabled: true, threshold: threshold, message: "msg"));

        decimal atThreshold = threshold;
        decimal justCrossing = Direction == ThresholdDirection.ActivatesAbove ? threshold + 0.01m : threshold - 0.01m;
        decimal justNotCrossing = Direction == ThresholdDirection.ActivatesAbove ? threshold - 0.01m : threshold + 0.01m;

        ConsoleCapture.Capture(writer =>
        {
            bot.Update(CreateWeatherData(atThreshold));
            writer.ToString().Should().BeEmpty("equal to threshold should never activate (strict comparison)");
            writer.GetStringBuilder().Clear();

            bot.Update(CreateWeatherData(justNotCrossing));
            writer.ToString().Should().BeEmpty();
            writer.GetStringBuilder().Clear();

            bot.Update(CreateWeatherData(justCrossing));
            writer.ToString().Should().Contain(ActivationKeyword);
        });
    }

    [Fact]
    public void Update_ShouldWritesConfiguredMessage_WhenEnabledActivates()
    {
        var bot = CreateBot(CreateConfig(enabled: true, threshold: 50m, message: "Custom message here"));
        var triggeringValue = Direction == ThresholdDirection.ActivatesAbove ? 51m : 49m;

        ConsoleCapture.Capture(writer =>
        {
            bot.Update(CreateWeatherData(triggeringValue));

            var output = writer.ToString();
            output.Should().Contain(ActivationKeyword);
            output.Should().Contain("Custom message here");
        });
    }
    [Fact]
    public void RainBot_UsesTemperatureThresholdNotHumidity_WhenConfigHasBothThresholdsSet()
    {
        var config = new BotConfigurations
        {
            Enabled = true,
            TemperatureThreshold = 100m,
            HumidityThreshold = 10m,
            Message = "msg"
        };
        var bot = new RainBot(config);
        var weather = new WeatherData { Humidity = 50m };

        ConsoleCapture.Capture(writer =>
        {
            bot.Update(weather);
            writer.ToString().Should().BeEmpty();
        });
    }
}