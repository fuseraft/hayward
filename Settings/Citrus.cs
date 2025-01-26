using System.Text.Json;
using System.Text.Json.Serialization;

namespace citrus.Settings;

public class Citrus
{
    public static CitrusSettings Settings { get; } = CitrusSettings.LoadCitrusSettings("citrus-settings.json");
}

public class CitrusSettings
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("version")]
    public string Version { get; set; } = string.Empty;

    [JsonPropertyName("file_extensions")]
    public required FileExtensions Extensions { get; set; }

    [JsonPropertyName("standard_library")]
    public List<StandardLibraryPath> StandardLibrary { get; set; } = [];

    [JsonPropertyName("safemode")]
    public bool SafeMode { get; set; } = false;

    [JsonPropertyName("debug")]
    public required DebugSettings Debug { get; set; }

    [JsonPropertyName("environment_variables")]
    public Dictionary<string, string> EnvironmentVariables { get; set; } = [];

    [JsonPropertyName("crashdump_path")]
    public string CrashDumpPath { get; set; } = string.Empty;

    public static CitrusSettings LoadCitrusSettings(string filePath)
    {
        return JsonSerializer.Deserialize<CitrusSettings>(File.ReadAllText(filePath), new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }
}

public class DebugSettings
{
    [JsonPropertyName("cli_args")]
    public List<string> CommandLineArguments { get; set; } = [];
}

public class StandardLibraryPath
{
    [JsonPropertyName("autoload")]
    public bool AutoLoad { get; set; } = true;

    [JsonPropertyName("path")]
    public string Path { get; set; } = string.Empty;

    [JsonPropertyName("include_subdirectories")]
    public bool IncludeSubdirectories { get; set; } = true;
}

public class FileExtensions
{
    [JsonPropertyName("primary")]
    public string Primary { get; set; } = string.Empty;

    [JsonPropertyName("minified")]
    public string Minified { get; set; } = string.Empty;

    [JsonPropertyName("recognized")]
    public List<string> Recognized { get; set; } = [];
}