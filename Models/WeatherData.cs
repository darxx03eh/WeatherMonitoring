namespace WeatherMonitoring.Models;

public sealed class WeatherData
{
    public string? Location  { get; set; }
    public decimal? Temperature { get; set; }
    public decimal? Humidity { get; set; }

    public override string ToString()
        => $"""
            Location: {this.Location}
            Temperature: {this.Temperature}
            Humidity: {this.Humidity}
            """;
}