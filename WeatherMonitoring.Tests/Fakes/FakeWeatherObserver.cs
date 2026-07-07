using WeatherMonitoring.Abstractions;
using WeatherMonitoring.Models;

namespace WeatherMonitoring.Tests.Fakes;

public class FakeWeatherObserver : IWeatherObserver
{
    private int _updateCount;
    public int UpdateCount => _updateCount;
    public WeatherData? LastReceivedData { get; private set; }
    private readonly Action<WeatherData>? _onUpdate;
    public FakeWeatherObserver(Action<WeatherData>? onUpdate = null)
        => _onUpdate = onUpdate;

    public void Update(WeatherData? weather)
    {
        Interlocked.Increment(ref _updateCount);
        LastReceivedData = weather;
        _onUpdate?.Invoke(weather);
    }
}