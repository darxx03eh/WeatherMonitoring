using System.Globalization;
using System.Reflection;
using CsvHelper;
using WeatherMonitoring.Tests.TestModels.Services;
using Xunit.Sdk;

namespace WeatherMonitoring.Tests.Attributes;

public class ObjectTypeDataAttribute : DataAttribute
{
    private readonly string _path;
    private readonly Type _type;
    public ObjectTypeDataAttribute(Type type, params string[] path)
        => (_type, _path) = (type, Path.Combine(path));
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "TestData", _path);

        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords(_type);
        foreach(var record in records)
            yield return [record];
    }
}