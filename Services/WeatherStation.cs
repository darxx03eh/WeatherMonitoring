using WeatherMonitoring.Abstractions;
using WeatherMonitoring.Models;

namespace WeatherMonitoring.Services;

public class WeatherStation : IWeatherStation
{
    private readonly List<IWeatherObserver> _observers;
    private readonly object _lock;
    public WeatherStation() => (_observers, _lock) = (new(), new());

    public void Register(IWeatherObserver observer)
    {
        ArgumentNullException.ThrowIfNull(observer);
        lock (_lock)
        {
            if(!_observers.Contains(observer))
                _observers.Add(observer);
        }
    }

    public void Unregister(IWeatherObserver observer)
    {
        ArgumentNullException.ThrowIfNull(observer);
        lock (_lock)
        {
            if(_observers.Contains(observer))
                _observers.Remove(observer);
        }
    }

    public void Publish(WeatherData data)
    {
        ArgumentNullException.ThrowIfNull(data);
        IWeatherObserver[] snapshot;
        lock (_lock)
        {
            snapshot = _observers.ToArray();
        }

        foreach (var observer in snapshot)
        {
            try
            {
                observer.Update(data);
            }
            catch (Exception exp)
            {
                Console.Error.WriteLine($"Observer {observer.GetType().Name} threw an exception: {exp.Message}");
            }
        }
    }
}