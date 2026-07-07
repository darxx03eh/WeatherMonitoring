using FluentAssertions;
using WeatherMonitoring.Models;
using WeatherMonitoring.Services;
using WeatherMonitoring.Tests.Fakes;
using WeatherMonitoring.Tests.Fixtures;

namespace WeatherMonitoring.Tests.Services;

public class WeatherStationTests : IClassFixture<WeatherStationFixture>
{
    public readonly WeatherStationFixture Fixture;
    public WeatherStationTests(WeatherStationFixture fixture) => Fixture = fixture;
    [Fact]
    public void Register_ShouldThrowArgumentNullException_WhenObserverIsNull()
    {
        Action act = () => Fixture.WeatherStation.Register(null);

        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("observer");
    }

    [Fact]
    public void Register_ShouldRegisterNewObserverAndIncreaseUpdateCountByOne_IfObserverIsNotNull()
    {
        var observer = new FakeWeatherObserver();
        
        Fixture.WeatherStation.Register(observer);
        Fixture.WeatherStation.Publish(new WeatherData());
        
        observer.UpdateCount.Should().Be(1);
    }

    [Fact]
    public void Register_ShouldRegisterNewObserverOnce_WhenSameObserverRegisteredTwice()
    {
        var observer = new FakeWeatherObserver();
        
        Fixture.WeatherStation.Register(observer);
        Fixture.WeatherStation.Register(observer);
        Fixture.WeatherStation.Publish(new WeatherData());
        
        observer.UpdateCount.Should().Be(1);
    }
    
    [Fact]
    public void Register_ShouldRegisterMultipleObservers_WhenObserversAreDifferent()
    {
        var firstObserver = new FakeWeatherObserver();
        var secondObserver = new FakeWeatherObserver();
        
        Fixture.WeatherStation.Register(firstObserver);
        Fixture.WeatherStation.Register(secondObserver);
        Fixture.WeatherStation.Publish(new WeatherData());
        
        firstObserver.UpdateCount.Should().Be(1);
        secondObserver.UpdateCount.Should().Be(1);
    }
    
    [Fact]
    public async Task Register_ShouldRegisterOneObserver_WhenCalledConcurrently_WithSameObserver()
    {
        var observer = new FakeWeatherObserver();

        var tasks = Enumerable.Range(0, 50)
            .Select(_ => Task.Run(() => Fixture.WeatherStation.Register(observer)))
            .ToArray();
        
        await Task.WhenAll(tasks);
        Fixture.WeatherStation.Publish(new WeatherData());
        
        observer.UpdateCount.Should().Be(1);
    }

    [Fact]
    public async Task Register_ShouldRegisterMultipleObservers_WhenCalledConcurrently()
    {
        var observers = Enumerable.Range(0, 100)
            .Select(_ => new FakeWeatherObserver())
            .ToList();

        var tasks = observers.Select(observer => Task.Run(() => Fixture.WeatherStation.Register(observer)));
        await Task.WhenAll(tasks);
        
        Fixture.WeatherStation.Publish(new WeatherData());
        
        observers.Should().OnlyContain(observer => observer.UpdateCount == 1);
    }

    [Fact]
    public void Unregister_ShouldThrowArgumentNullException_WhenObserverIsNull()
    {
        Action act = () => Fixture.WeatherStation.Unregister(null);

        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("observer");
    }
    
    [Fact]
    public void Unregister_ShouldUnregisterExistingObserverAndDecreaseUpdateCountByOne_IfObserverIsNotNull()
    {
        var observer = new FakeWeatherObserver();
        
        Fixture.WeatherStation.Register(observer);
        Fixture.WeatherStation.Unregister(observer);
        Fixture.WeatherStation.Publish(new WeatherData());
        
        observer.UpdateCount.Should().Be(0);
    }

    [Fact]
    public void Unregister_ShouldUnregisterExistingObserverOnce_WhenSameObserverUnregisteredTwice()
    {
        var observer = new FakeWeatherObserver();
        
        Fixture.WeatherStation.Register(observer);
        Fixture.WeatherStation.Unregister(observer);
        Fixture.WeatherStation.Unregister(observer);
        Fixture.WeatherStation.Publish(new WeatherData());
        
        observer.UpdateCount.Should().Be(0);
    }

    [Fact]
    public void Unregister_ShouldUnregisterMultipleObservers_WhenObserversAreDifferent()
    {
        var firstObserver = new FakeWeatherObserver();
        var secondObserver = new FakeWeatherObserver();
        
        Fixture.WeatherStation.Register(firstObserver);
        Fixture.WeatherStation.Register(secondObserver);
        Fixture.WeatherStation.Unregister(firstObserver);
        Fixture.WeatherStation.Unregister(secondObserver);
        
        Fixture.WeatherStation.Publish(new WeatherData());
        firstObserver.UpdateCount.Should().Be(0);
        secondObserver.UpdateCount.Should().Be(0);
    }

    [Fact]
    public async Task Unregister_ShouldUnregisterOneObserver_WhenCalledConcurrently_WithSameObserver()
    {
        var observer = new FakeWeatherObserver();

        var registerTasks = Enumerable.Range(0, 50)
            .Select(_ => Task.Run(() => Fixture.WeatherStation.Register(observer)))
            .ToArray();
        
        await Task.WhenAll(registerTasks);
        var unregisterTasks = Enumerable.Range(0, 50)
            .Select(_ => Task.Run(() => Fixture.WeatherStation.Unregister(observer)))
            .ToArray();
        
        await Task.WhenAll(unregisterTasks);
        Fixture.WeatherStation.Publish(new WeatherData());
        observer.UpdateCount.Should().Be(0);
    }

    [Fact]
    public async Task Unregister_ShouldUnregisterMultipleObservers_WhenCalledConcurrently()
    {
        var observers = Enumerable.Range(0, 100)
            .Select(_ => new FakeWeatherObserver())
            .ToList();
        
        var registerTasks = observers.Select(observer => Task.Run(() => Fixture.WeatherStation.Register(observer)));
        await Task.WhenAll(registerTasks);
        var unregisterTasks = observers.Select(observer => Task.Run(() => Fixture.WeatherStation.Unregister(observer)));
        
        await Task.WhenAll(unregisterTasks);
        Fixture.WeatherStation.Publish(new WeatherData());
        
        observers.Should().OnlyContain(observer => observer.UpdateCount == 0);
    }

    [Fact]
    public void Publish_ShouldThrowArgumentNullException_WhenWeatherDataIsNull()
    {
        Action act = () => Fixture.WeatherStation.Publish(null);
        
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("data");
    }

    [Fact]
    public void Publish_ShouldNotThrowException_WhenThereIsNoObserversRegisterd()
    {
        Action act = () => Fixture.WeatherStation.Publish(new WeatherData());

        act.Should().NotThrow();
    }

    [Fact]
    public void Publish_ShouldAllObserversReceivedSameWeatherData()
    {
        var firstObserver = new FakeWeatherObserver();
        var secondObserver = new FakeWeatherObserver();
        
        Fixture.WeatherStation.Register(firstObserver);
        Fixture.WeatherStation.Register(secondObserver);
        var data = new WeatherData();
        Fixture.WeatherStation.Publish(data);
        
        firstObserver.UpdateCount.Should().Be(1);
        secondObserver.UpdateCount.Should().Be(1);
        firstObserver.LastReceivedData.Should().BeSameAs(data);
        secondObserver.LastReceivedData.Should().BeSameAs(data);
    }

    [Fact]
    public void Publish_ShouldNotifiesAllObservers_WhenPublishCalledMultipleTime()
    {
        var observer = new FakeWeatherObserver();
        
        Fixture.WeatherStation.Register(observer);
        
        Fixture.WeatherStation.Publish(new WeatherData());
        Fixture.WeatherStation.Publish(new WeatherData());
        Fixture.WeatherStation.Publish(new WeatherData());
        
        observer.UpdateCount.Should().Be(3);
    }
    
    [Fact]
    public void Publish_ShouldStillNotifiesAllObservers_WhenObserverThrows()
    {
        var observer = new FakeWeatherObserver();
        
        var firstObserver = new FakeWeatherObserver();
        var throwingObserver = new ThrowingWeatherObserver();
        var secondObserver = new FakeWeatherObserver();
        
        Fixture.WeatherStation.Register(firstObserver);
        Fixture.WeatherStation.Register(throwingObserver);
        Fixture.WeatherStation.Register(secondObserver);
        
        Action act = () =>  Fixture.WeatherStation.Publish(new WeatherData());
        act.Should().NotThrow();
        firstObserver.UpdateCount.Should().Be(1);
        secondObserver.UpdateCount.Should().Be(1);
    }

    [Fact]
    public void Publish_ShouldNotThrow_WhenAllObserversThrow()
    {
        Fixture.WeatherStation.Register(new ThrowingWeatherObserver());
        Fixture.WeatherStation.Register(new ThrowingWeatherObserver());
        
        Action act =  () =>  Fixture.WeatherStation.Publish(new WeatherData());

        act.Should().NotThrow();
    }

    [Fact]
    public void Publish_ObserverShouldBeableToUnregistersItselfDuringUpdate_CurrentPublishStillNotifiesIt()
    {
        FakeWeatherObserver? self = null;
        self = new FakeWeatherObserver(_ => Fixture.WeatherStation.Unregister(self!));
        
        Fixture.WeatherStation.Register(self);
        
        Fixture.WeatherStation.Publish(new WeatherData());
        self.UpdateCount.Should().Be(1);
        
        Fixture.WeatherStation.Publish(new WeatherData());
        // should still 1, count unchanged after unregister it self
        self.UpdateCount.Should().Be(1);
    }

    [Fact]
    public void Publish_ObserverShouldBeableToRegisterNewObserversDuringUpdate_NewObserversNotNotifiesThisRound()
    {
        var lateJoiner = new FakeWeatherObserver();
        var observer = new FakeWeatherObserver(_ => Fixture.WeatherStation.Register(lateJoiner));
        
        Fixture.WeatherStation.Register(observer);
        Fixture.WeatherStation.Publish(new WeatherData());
        
        lateJoiner.UpdateCount.Should().Be(0);
        observer.UpdateCount.Should().Be(1);
        
        Fixture.WeatherStation.Publish(new WeatherData());
        lateJoiner.UpdateCount.Should().Be(1);
        observer.UpdateCount.Should().Be(2);
    }

    [Fact]
    public async Task Publish_ShouldDoesNotThrow_WhenCalledConcurrentlyWithRegisterAndUnregister()
    {
        var observers = Enumerable.Range(0, 50)
            .Select(_ => new FakeWeatherObserver())
            .ToList();
        observers.ForEach(Fixture.WeatherStation.Register);
        var publishTasks = Enumerable.Range(0, 50)
            .Select(_ => Task.Run(() => Fixture.WeatherStation.Publish(new WeatherData())));
        var registerTasks = Enumerable.Range(0, 25)
            .Select(_ => Task.Run(() => Fixture.WeatherStation.Register(new FakeWeatherObserver())));
        var unregisterTasks = observers.Take(25)
            .Select(observer => Task.Run(() => Fixture.WeatherStation.Unregister(observer)));
        
        Func<Task> act = () => Task.WhenAll(publishTasks.Concat(registerTasks).Concat(unregisterTasks));

        await act.Should().NotThrowAsync();
    }
}