using WeatherMonitoring.Abstractions;
using WeatherMonitoring.Models;

namespace WeatherMonitoring.Tests.Fakes;

public class ThrowingWeatherObserver : IWeatherObserver
{
    public void Update(WeatherData? weather) => throw new InvalidOperationException("Simulated failure");
}