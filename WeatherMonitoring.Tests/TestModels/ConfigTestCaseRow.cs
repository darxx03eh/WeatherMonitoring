namespace WeatherMonitoring.Tests.TestModels;

public sealed class ConfigTestCaseRow
{
    public string Input { get; set; } = "";
    public bool RainBotEnabled { get; set; }
    public decimal RainBotThreshold { get; set; }
    public string RainBotMessage { get; set; } = "";
    public bool SunBotEnabled { get; set; }
    public decimal SunBotThreshold { get; set; }
    public string SunBotMessage { get; set; } = "";
    public bool SnowBotEnabled { get; set; }
    public decimal SnowBotThreshold { get; set; }
    public string SnowBotMessage { get; set; } = "";
}