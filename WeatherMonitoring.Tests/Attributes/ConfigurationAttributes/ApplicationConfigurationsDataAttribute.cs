using System.Globalization;
using System.Reflection;
using CsvHelper;
using WeatherMonitoring.Tests.TestModels;
using Xunit.Sdk;

namespace WeatherMonitoring.Tests.Attributes.ConfigurationAttributes;

public class ApplicationConfigurationsDataAttribute : DataAttribute
{
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        var path = Path.Combine(AppContext.BaseDirectory,
            "TestData",
            "Services", "ConfigurationsLoader",
            "config-test-data.csv");

        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<ConfigTestCaseRow>();
        foreach(var record in records)
            yield return [record];
    }
}