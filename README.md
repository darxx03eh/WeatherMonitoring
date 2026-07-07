# WeatherMonitoring

A .NET console application that monitors weather data and activates notification bots based on configurable thresholds. Built with C# 10, .NET 10.0, and the **Observer design pattern**.

## Overview

WeatherMonitoring reads weather data (temperature and humidity) from console input in JSON or XML format, parses it, and notifies registered observer bots when configured thresholds are exceeded. Each bot reacts independently based on its own configuration.

## Architecture

The system follows the **Observer pattern** (also known as Publish/Subscribe):

```
┌─────────────┐      ┌──────────────────┐      ┌─────────┐
│ Console Input│─────▶│  ParserFactory   │─────▶│ WeatherData│
└─────────────┘      └──────────────────┘      └────┬────┘
                                                     │
                                                     ▼
                                              ┌──────────────┐
                                              │WeatherStation│
                                              │  (Publisher) │
                                              └──────┬───────┘
                                                     │
                              ┌───────────────────────┼───────────────────────┐
                              ▼                       ▼                       ▼
                       ┌───────────┐           ┌───────────┐           ┌───────────┐
                       │  RainBot  │           │  SnowBot  │           │  SunBot   │
                       │ (Observer)│           │ (Observer)│           │ (Observer)│
                       └───────────┘           └───────────┘           └───────────┘
```

### Core Components

| Component | Description |
|-----------|-------------|
| **`WeatherStation`** | Central publisher that manages observer registration and broadcasts weather updates. Thread-safe with snapshot-based notification. |
| **`RainBot`** | Activates when humidity exceeds the configured threshold. |
| **`SnowBot`** | Activates when temperature drops below the configured threshold. |
| **`SunBot`** | Activates when temperature exceeds the configured threshold. |
| **`ParserFactory`** | Auto-detects input format (JSON or XML) and returns the appropriate parser. Uses `OneOf` for discriminated union return types. |
| **`ConfigurationsLoader`** | Loads bot configurations from a JSON file. |
| **`WeatherData`** | Data model containing `Location`, `Temperature`, and `Humidity`. |

## Project Structure

```
WeatherMonitoring/
├── Program.cs                          # Application entry point
├── Abstractions/                       # Interfaces
│   ├── IWeatherObserver.cs             # Observer contract
│   ├── IWeatherParser.cs               # Parser contract
│   └── IWeatherStation.cs              # Station contract
├── Bots/                               # Observer implementations
│   ├── RainBot.cs                      # Humidity-based observer
│   ├── SnowBot.cs                      # Cold temperature observer
│   └── SunBot.cs                       # Hot temperature observer
├── Configurations/                     # Configuration models
│   ├── ApplicationConfigurations.cs    # Root configuration
│   ├── BotConfigurations.cs            # Per-bot settings
│   └── configs.json                    # Default configuration file
├── Models/
│   └── WeatherData.cs                  # Weather data model
├── Parsers/                            # Input parsers
│   ├── JsonWeatherParser.cs            # JSON parser
│   ├── XmlWeatherParser.cs             # XML parser
│   └── ParserFactory.cs               # Format detection & parser selection
├── Services/                           # Core services
│   ├── WeatherStation.cs               # Observer pattern publisher
│   └── ConfigurationsLoader.cs         # Configuration file loader
└── WeatherMonitoring.Tests/            # Unit tests
    ├── Bots/                           # Bot tests
    ├── Parsers/                        # Parser tests
    ├── Services/                       # Service tests
    ├── Fakes/                          # Test doubles
    ├── Fixtures/                       # Shared test setup
    └── TestData/                       # CSV-based test data
```

## Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later

## Getting Started

### Build

```bash
dotnet build
```

### Run

```bash
dotnet run
```

The application will prompt for weather data input. Provide data in JSON or XML format.

### Example Input

**JSON:**
```json
{"Location": "New York", "Temperature": 35, "Humidity": 80}
```

**XML:**
```xml
<WeatherData>
  <Location>New York</Location>
  <Temperature>35</Temperature>
  <Humidity>80</Humidity>
</WeatherData>
```

### Example Output

```
Enter weather data: {"Location": "New York", "Temperature": 35, "Humidity": 80}
RainBot activated!
RainBot: It looks like it's about to pour down!
SunBot activated!
SunBot: Wow, it's a scorcher out there!
```

## Configuration

Bot behavior is controlled via `Configurations/configs.json`:

```json
{
  "RainBot": {
    "enabled": true,
    "humidityThreshold": 70,
    "message": "It looks like it's about to pour down!"
  },
  "SunBot": {
    "enabled": true,
    "temperatureThreshold": 30,
    "message": "Wow, it's a scorcher out there!"
  },
  "SnowBot": {
    "enabled": false,
    "temperatureThreshold": 0,
    "message": "Brrr, it's getting chilly!"
  }
}
```

| Field | Description |
|-------|-------------|
| `enabled` | Whether the bot is active (`true`/`false`). |
| `humidityThreshold` | Humidity value above which RainBot activates. |
| `temperatureThreshold` | Temperature threshold for SunBot (above) or SnowBot (below). |
| `message` | Custom message displayed when the bot activates. |

A custom configuration file can be loaded by passing a path to `ConfigurationsLoader.LoadConfigurations(path)`.

## Testing

### Run Tests

```bash
dotnet test
```

### Test Framework

- **xUnit** - Test framework
- **FluentAssertions** - Assertion library
- **CsvHelper** - Data-driven test support via CSV files
- **Coverlet** - Code coverage

### Test Structure

Tests are organized by component and use CSV-based test data for data-driven scenarios:

- **Bot Tests** - Verify activation/deactivation logic for each bot
- **Parser Tests** - Validate JSON and XML parsing
- **ParserFactory Tests** - Test format detection and error handling
- **ConfigurationsLoader Tests** - Test config loading and error cases

Custom test attributes:
- `CsvDataAttribute` - Loads test cases from CSV files
- `NullOrWhiteSpaceDataAttribute` - Generates null/empty/whitespace test cases

## CI/CD

A GitHub Actions workflow (`.github/workflows/ci.yml`) runs on pushes and pull requests to `main` and `develop` branches:

1. Restores dependencies
2. Builds the project
3. Runs all tests

## Key Design Decisions

- **Observer Pattern** - Decouples weather data publication from bot notification logic. New bots can be added without modifying the station.
- **Thread-Safe Station** - `WeatherStation` uses locking and snapshot-based iteration, allowing safe concurrent registration/unregistration during publish.
- **Discriminated Unions** - `ParserFactory` uses the `OneOf` library to return typed results (`IWeatherParser`, `ArgumentException`, or `NotSupportedException`) without exceptions.
- **Auto-Detection** - Input format (JSON vs XML) is detected automatically from the first character of the input.
- **Configuration-Driven** - Bot thresholds, enabled state, and messages are externalized to a JSON config file.