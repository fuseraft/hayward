using System.Text.Json;
using System.Text.Json.Serialization;

namespace hayward.Settings;

public class Hayward
{
    public static HaywardSettings Settings { get; } = HaywardSettings.LoadHaywardSettings("hayward-settings.json");
}

public class HaywardSettings
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

    public static HaywardSettings LoadHaywardSettings(string filePath)
    {
        var binPath = Path.GetDirectoryName(Environment.ProcessPath) ?? string.Empty;
        var settingsPath = Path.Combine(binPath, filePath);

        if (File.Exists(settingsPath))
        {
            return JsonSerializer.Deserialize<HaywardSettings>(File.ReadAllText(settingsPath), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        }

        Console.WriteLine("Could not find hayward-settings.json. Run with --settings to view defaults.");

        return new HaywardSettings
        {
            Name = "hayward",
            Version = "1.1.3",
            SafeMode = true,
            Extensions = new FileExtensions {
                Primary = ".kiwi",
                Minified = ".min.kiwi",
                Recognized = [".hayward", ".kiwi"]
            },
            Debug = new DebugSettings {},
            CrashDumpPath = "hayward-crash.log"
        };
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

    [JsonPropertyName("is_override")]
    public bool IsOverride { get; set; } = false;
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