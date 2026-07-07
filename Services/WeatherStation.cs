using WeatherMonitoring.Abstractions;
using WeatherMonitoring.Models;

namespace WeatherMonitoring.Services;

public class WeatherStation : IWeatherStation
{
    private readonly List<IWeatherObserver> _observers;
    private readonly object _lock;
    private int _observerCount;
    public int ObserverCount => _observerCount;
    public WeatherStation() => (_observers, _lock) = (new(), new());
    /// <summary>
    /// Registers an observer to receive weather data updates.
    /// If the observer is already registered, this method does nothing (no duplicate subscriptions).
    /// This method is thread-safe
    /// </summary>
    /// <param name="observer">The observer to register. Cannot be <see langword="null"/>.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="observer"/> is <see langword="null"/>.</exception>
    public void Register(IWeatherObserver? observer)
    {
        ArgumentNullException.ThrowIfNull(observer);
        lock (_lock)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
                Interlocked.Increment(ref _observerCount);
            }
        }
    }
    /// <summary>
    /// Removes a previously registered observer so it no longer receives weather data updates.
    /// If the observer was not registered, this method does nothing.
    /// This method is thread-safe.
    /// </summary>
    /// <param name="observer">The observer to Unregister. Cannot be <see langword="null"/>.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="observer"/> is <see langword="null"/>.</exception>
    public void Unregister(IWeatherObserver? observer)
    {
        ArgumentNullException.ThrowIfNull(observer);
        lock (_lock)
        {
            if (_observers.Contains(observer))
            {
                _observers.Remove(observer);
                Interlocked.Decrement(ref _observerCount);
            }
        }
    }
    /// <summary>
    /// Notifies all registered observers of new weather data.
    /// A snapshot of the observer list is taken before notification, so observers may safely
    /// register or unregister themselves during their own <see cref="IWeatherObserver.Update"/> call
    /// without affecting the current publish operation.
    /// If an observer throws during notification, the exception is caught and logged so that
    /// remaining observers still receive the update.
    /// This method is thread-safe.
    /// </summary>
    /// <param name="data"></param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="data"/> is <see langword="null"/>.</exception>
    public void Publish(WeatherData? data)
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