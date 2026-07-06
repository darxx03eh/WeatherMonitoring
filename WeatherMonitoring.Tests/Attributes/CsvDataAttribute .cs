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
    public ObjectTypeDataAttribute(string path,  Type type) => (_path, _type) = (path, type);
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