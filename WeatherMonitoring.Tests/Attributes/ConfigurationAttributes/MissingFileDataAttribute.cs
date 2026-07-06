using System.Globalization;
using System.Reflection;
using CsvHelper;
using WeatherMonitoring.Tests.TestModels;
using Xunit.Sdk;

namespace WeatherMonitoring.Tests.Attributes.ConfigurationAttributes;

public class MissingFileDataAttribute : DataAttribute
{
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        var path = Path.Combine(AppContext.BaseDirectory,
            "TestData",
            "Services", "ConfigurationsLoader",
            "missing-file-test-data.csv");

        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<MissingFileTestCaseRow>();
        foreach (var record in records)
            yield return [record.Path];
    }
}