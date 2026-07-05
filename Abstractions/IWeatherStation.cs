using WeatherMonitoring.Models;

namespace WeatherMonitoring.Abstractions;

public interface IWeatherStation
{
    void Register(IWeatherObserver? observer);
    void Unregister(IWeatherObserver? observer);
    void Publish(WeatherData? data);
}