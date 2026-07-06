using System.Globalization;
using System.Reflection;
using CsvHelper;
using WeatherMonitoring.Tests.TestModels.Parsers;
using Xunit.Sdk;

namespace WeatherMonitoring.Tests.Attributes.ParserAttributes;

public class ValidJsonDataAttribute : DataAttribute
{
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        var path = Path.Combine(AppContext.BaseDirectory,
            "TestData",
            "Parsers", "JsonWeatherParser",
            "valid-json-test-data.csv");

        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<WeatherDataTestCaseRow>();

        foreach (var record in records)
            yield return [record];
    }
}