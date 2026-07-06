using System.Reflection;
using Xunit.Sdk;

namespace WeatherMonitoring.Tests.Attributes;

public class NullOrWhiteSpaceDataAttribute : DataAttribute
{
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        yield return [""];
        yield return ["    "];
        yield return [null!];
    }
}