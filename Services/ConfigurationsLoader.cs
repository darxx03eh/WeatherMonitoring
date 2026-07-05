using System.Text;
using System.Text.Json;
using WeatherMonitoring.Configurations;

namespace WeatherMonitoring.Services;

public static class ConfigurationsLoader
{
    public static ApplicationConfigurations? LoadConfigurations()
    {
        string path = @$"{Directory.GetCurrentDirectory()}\Configurations\configs.json";
        string botConfigs = File.ReadAllText(path, Encoding.UTF8);
        return JsonSerializer.Deserialize<ApplicationConfigurations>(
            botConfigs, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });
    }
}