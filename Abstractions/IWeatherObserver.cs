using WeatherMonitoring.Models;

namespace WeatherMonitoring.Abstractions;

public interface IWeatherObserver
{
    void Update(WeatherData? weather);
}