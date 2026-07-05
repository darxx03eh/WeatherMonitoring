using System.Text.Json.Serialization;

namespace WeatherMonitoring.Configurations;

public sealed class BotConfigurations
{
    public bool Enabled { get; set; }
    public decimal? TemperatureThreshold { get; set; }
    public decimal? HumidityThreshold { get; set; }
    [JsonIgnore]
    public decimal Threshold => TemperatureThreshold ?? HumidityThreshold ?? 0.0m;
    public string? Message { get; set; }
}