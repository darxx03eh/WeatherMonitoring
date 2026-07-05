namespace WeatherMonitoring.Configurations;

public sealed class ApplicationConfigurations
{
    public BotConfigurations? RainBot { get; set; }
    public BotConfigurations? SunBot { get; set; }
    public BotConfigurations? SnowBot { get; set; }
}