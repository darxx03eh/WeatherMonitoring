namespace WeatherMonitoring.Tests.TestModels.Parsers;

public sealed class WeatherDataTestCaseRow
{
    public string Input { get; set; } = "";
    public string ExpectedLocation { get; set; } = "";
    public decimal ExpectedTemperature { get; set; }
    public decimal ExpectedHumidity { get; set; }
}