using System.Text;
using System.Text.Json;
using WeatherMonitoring.Configurations;

namespace WeatherMonitoring.Services;

public static class ConfigurationsLoader
{
    /// <summary>
    /// Reads configuration from the given path (or the default Configurations\configs.json
    /// relative to the application base directory if no path is provided), then deserializes
    /// it into an <see cref="ApplicationConfigurations"/> object.
    /// </summary>
    /// <param name="path">
    /// Optional path to the configuration file. If <see langword="null"/>, defaults to
    /// Configurations\configs.json relative to the application base directory.
    /// </param>
    /// <returns>
    /// The deserialized <see cref="ApplicationConfigurations"/>, or <see langword="null"/> if the JSON
    /// content deserializes to null.
    /// </returns>
    /// <exception cref="FileNotFoundException">Thrown when the configuration file does not exist at the given path.</exception>
    /// <exception cref="JsonException">Thrown when the file content is not valid JSON or does not match the expected shape.</exception>
    public static ApplicationConfigurations? LoadConfigurations(string? path = null)
    {
        path ??= Path.Combine(AppContext.BaseDirectory, "Configurations", "configs.json");
        if (!File.Exists(path)) throw new FileNotFoundException($"Configuration file not found at: {path}");
        string botConfigs = File.ReadAllText(path, Encoding.UTF8);
        return Deserialize(botConfigs);
    }

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
    public static ApplicationConfigurations? Deserialize(string json)
        => JsonSerializer.Deserialize<ApplicationConfigurations>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
}