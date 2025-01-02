using citrus.Parsing;
using citrus.Tracing.Error;
namespace citrus.Builtin;

public struct FileUtil
{
    public static bool DirectoryExists(Token token, string path)
    {
        try
        {
            return File.Exists(path) && Directory.Exists(path);
        }
        catch (Exception)
        {
            throw new FileSystemError(token, $"Could not determine if directory exists: {path}");
        }
    }

    public static bool FileExists(Token token, string filePath)
    {
        try
        {
            return File.Exists(filePath);
        }
        catch (Exception)
        {
            throw new FileSystemError(token, $"Could not determine if file exists: {filePath}");
        }
    }

    public static string GetAbsolutePath(Token token, string path)
    {
        try 
        {
            return Path.GetFullPath(path);
        }
        catch (Exception)
        {
            throw new FileSystemError(token, $"Could not get absolute path: {path}");
        }
    }

    public static string GetFileExtension(Token token, string filePath)
    {
        try
        {
            return Path.GetExtension(filePath);
        }
        catch (Exception)
        {
            throw new FileSystemError(token, $"Could not get extension for file: {filePath}");
        }
    }

    public static bool IsScript(Token token, string path)
    {
        List<string> validExtensions = [".min.kiwi", ".kiwi", ".min.🥝", ".🥝", ".min.k", ".k"];

        var extension = GetFileExtension(token, path);
        return validExtensions.Contains(extension);
    }

    public static string ReadFile(Token token, string filePath)
    {
        try
        {
            return File.ReadAllText(filePath);
        }
        catch (Exception)
        {
            throw new FileReadError(token, filePath);
        }
    }

    public static string TryGetExtensionless(Token token, string path)
    {
        if (IsScript(token, path) && FileExists(token, path) && !DirectoryExists(token, path))
        {
            return path;
        }

        List<string> validExtensions = [".min.kiwi", ".kiwi", ".min.🥝", ".🥝", ".min.k", ".k"];

        foreach (var ext in validExtensions)
        {
            var scriptPath = TryGetExtensionlessSpecific(token, path, ext);
            if (!string.IsNullOrEmpty(scriptPath))
            {
                return scriptPath;
            }
        }

        return string.Empty;
    }

    public static string TryGetExtensionlessSpecific(Token token, string path, string extension)
    {
        var scriptPath = path + extension;
        if (FileExists(token, scriptPath))
        {
            return scriptPath;
        }
        return string.Empty;
    }
}