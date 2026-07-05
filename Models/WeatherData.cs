namespace WeatherMonitoring.Models;

public class WeatherData
{
    public string? Location  { get; set; }
    public decimal? Temperature { get; set; }
    public decimal? Humidity { get; set; }
}