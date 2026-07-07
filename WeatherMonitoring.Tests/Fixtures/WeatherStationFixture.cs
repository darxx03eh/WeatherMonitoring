using WeatherMonitoring.Services;

namespace WeatherMonitoring.Tests.Fixtures;

public class WeatherStationFixture : IDisposable
{
    public WeatherStation WeatherStation { get; private set; }
    public WeatherStationFixture() => WeatherStation = new WeatherStation();
    public void Dispose()
    {
        //todo implement dispose method
    }
}