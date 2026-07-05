using System.Text;
using System.Text.Json;
using WeatherMonitoring.Configurations;

namespace WeatherMonitoring.Services;

public static class ConfigurationsLoader
{
    /// <summary>
    /// Reads configuration from Configurations\configs.json relative to the application base directory,
    /// then deserializes it into an <see cref="ApplicationConfigurations"/> object.
    /// </summary>
    /// <returns>
    /// The deserialized <see cref="ApplicationConfigurations"/>, or <see langword="null"/> if the JSON
    /// content deserializes to null.
    /// </returns>
    /// <exception cref="FileNotFoundException">Thrown when the configuration file does not exist at the expected path.</exception>
    /// <exception cref="JsonException">Thrown when the file content is not valid JSON or does not match the expected shape.</exception>
    public static ApplicationConfigurations? LoadConfigurations()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "Configurations", "configs.json");
        if(!File.Exists(path)) throw  new FileNotFoundException($"Configuration file not found at: {path}");
        string botConfigs = File.ReadAllText(path, Encoding.UTF8);
        return JsonSerializer.Deserialize<ApplicationConfigurations>(
            botConfigs, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });
    }
}