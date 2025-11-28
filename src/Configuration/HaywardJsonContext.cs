using System.Text.Json;
using System.Text.Json.Serialization;

namespace hayward.Settings;

[JsonSourceGenerationOptions(
    WriteIndented = true,
    PropertyNameCaseInsensitive = true
)]
[JsonSerializable(typeof(HaywardSettings))]
[JsonSerializable(typeof(FileExtensions))]
[JsonSerializable(typeof(DebugSettings))]
[JsonSerializable(typeof(StandardLibraryPath))]
[JsonSerializable(typeof(List<StandardLibraryPath>))]
[JsonSerializable(typeof(Dictionary<string, string>))]
[JsonSerializable(typeof(List<string>))]
[JsonSerializable(typeof(Config))]
partial class HaywardJsonContext : JsonSerializerContext { }