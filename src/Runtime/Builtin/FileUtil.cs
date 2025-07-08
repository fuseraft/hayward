using hayward.Parsing;
using hayward.Settings;
using hayward.Tracing.Error;
using hayward.Typing;
using Microsoft.Extensions.FileSystemGlobbing;

namespace hayward.Runtime.Builtin;

public struct FileUtil
{
    public static string CombinePath(Token token, List<string> parts)
    {
        try
        {
            var path = Path.Combine([.. parts]);
            return path;
        }
        catch (Exception)
        {
            throw new InvalidOperationError(token, $"Could not combine one or more parts: {string.Join(", ", parts)}");
        }
    }

    public static long GetFileSize(Token token, string path)
    {
        try
        {
            return new FileInfo(path).Length;
        }
        catch (Exception)
        {
            throw new FileReadError(token, path);
        }
    }

    public static List<Value> Glob(Token token, string path, List<string> includePatterns, List<string> excludePatterns)
    {
        try
        {
            Matcher matcher = new();
            matcher.AddIncludePatterns([.. includePatterns]);

            if (excludePatterns.Count > 0)
            {
                matcher.AddExcludePatterns([.. excludePatterns]);
            }

            var files = matcher.GetResultsInFullPath(path);
            return files.Select(x => Value.CreateString(x)).ToList();
        }
        catch (Exception)
        {
            throw new FileSystemError(token, $"Could not glob: {path}");
        }
    }

    public static Value GetFileInfo(Token token, string path)
    {
        try
        {
            Dictionary<Value, Value> metaData = [];
            FileSystemInfo fsi = Directory.Exists(path) ? new DirectoryInfo(path) : File.Exists(path) ? new FileInfo(path) : throw new FileSystemError(token);

            metaData.Add(Value.CreateString("creation_time"), Value.CreateDate(fsi.CreationTime));
            metaData.Add(Value.CreateString("name"), Value.CreateString(fsi.Name));
            metaData.Add(Value.CreateString("extension"), Value.CreateString(fsi.Extension));
            metaData.Add(Value.CreateString("full_name"), Value.CreateString(fsi.FullName));
            metaData.Add(Value.CreateString("last_access_time"), Value.CreateDate(fsi.LastAccessTime));
            metaData.Add(Value.CreateString("last_write_time"), Value.CreateDate(fsi.LastWriteTime));

            Dictionary<Value, Value> attributes = [];
            foreach (var attr in Enum.GetValues<FileAttributes>())
            {
                if (attr == 0)
                {
                    continue;
                }

                var isSet = (fsi.Attributes & attr) == attr;
                attributes[Value.CreateString(attr.ToString())] = Value.CreateBoolean(isSet);
            }

            metaData.Add(Value.CreateString("attributes"), Value.CreateHashmap(attributes));

            return Value.CreateHashmap(metaData);
        }
        catch (Exception)
        {
            throw new FileSystemError(token, $"Could not list path metadata: {path}");
        }
    }

    public static List<Value> ListDirectory(Token token, string path, bool recursive = false)
    {
        try
        {
            var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            return Directory.EnumerateFiles(path, "*.*", searchOption).Select(x => Value.CreateString(x)).ToList();
        }
        catch (Exception)
        {
            throw new FileSystemError(token, $"Could not list directory contents: {path}");
        }
    }

    public static bool MoveFile(Token token, string sourcePath, string destinationPath, bool overwrite = true)
    {
        try
        {
            File.Move(sourcePath, destinationPath, overwrite);
            return true;
        }
        catch (Exception)
        {
            throw new FileSystemError(token, $"Could not move file: {sourcePath}");
        }
    }

    public static bool CopyFile(Token token, string sourcePath, string destinationPath, bool overwrite = true)
    {
        try
        {
            File.Copy(sourcePath, destinationPath, overwrite);
            return true;
        }
        catch (Exception)
        {
            throw new FileSystemError(token, $"Could not copy file: {sourcePath}");
        }
    }

    public static bool CopyDirectoryRecursive(Token token, string sourcePath, string destinationPath)
    {
        try
        {
            if (!Directory.Exists(sourcePath))
            {
                return false;
            }

            Directory.CreateDirectory(destinationPath);

            var files = Directory.GetFiles(sourcePath);
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                var destFileName = Path.Combine(destinationPath, fileName);

                File.Copy(file, destFileName, overwrite: true);
            }

            var dirs = Directory.GetDirectories(sourcePath);
            foreach (var dir in dirs)
            {
                var dirName = Path.GetFileName(dir);
                var destDirName = Path.Combine(destinationPath, dirName);

                CopyDirectoryRecursive(token, dir, destDirName);
            }

            return true;
        }
        catch (Exception)
        {
            throw new FileSystemError(token, $"Could not copy directory: {sourcePath}");
        }
    }

    public static bool CreateFile(Token token, string path)
    {
        try
        {
            using var fio = File.Create(path);
            return File.Exists(path);
        }
        catch (Exception)
        {
            throw new FileSystemError(token, $"Could not create file: {path}");
        }
    }

    public static bool DirectoryExists(Token token, string path)
    {
        try
        {
            return Directory.Exists(path);
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

    public static string GetCurrentDirectory()
    {
        return Environment.CurrentDirectory;
    }

    public static string ExpandPath(string expand)
    {
        var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var path = expand.Replace("~", home).Replace("//", "/");
        return Path.GetFullPath(path);
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

    public static string GetParentPath(Token token, string path)
    {
        try
        {
            var parent = Directory.GetParent(path)?.FullName;

            if (string.IsNullOrEmpty(parent))
            {
                throw new FileSystemError(token, $"Could not get parent path: {path}");
            }

            return parent;
        }
        catch (Exception)
        {
            throw new FileSystemError(token, $"Could not get parent path path: {path}");
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

    public static string GetFileName(Token token, string filePath)
    {
        try
        {
            return Path.GetFileName(filePath);
        }
        catch (Exception)
        {
            throw new FileSystemError(token, $"Could not get file name for path: {filePath}");
        }
    }

    public static bool ChangeDirectory(Token token, string path)
    {
        try
        {
            Directory.SetCurrentDirectory(path);
            return true;
        }
        catch (Exception)
        {
            throw new FileSystemError(token, $"Could not change directory: {path}");
        }
    }

    public static bool MakeDirectory(Token token, string path)
    {
        try
        {
            Directory.CreateDirectory(path);
            return true;
        }
        catch (Exception)
        {
            throw new FileSystemError(token, $"Could not create path: {path}");
        }
    }

    public static string GetTempDirectory()
    {
        return Path.GetTempPath();
    }

    public static bool RemovePath(Token token, string path)
    {
        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }
            else if (Directory.Exists(path))
            {
                Directory.Delete(path, recursive: false);
                return true;
            }

            return false;
        }
        catch (Exception)
        {
            throw new FileSystemError(token, $"Could not remove path: {path}");
        }
    }

    public static int RemovePathRecursive(Token token, string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
            return 1;
        }
        else if (Directory.Exists(path))
        {
            return DeleteDirectoryRecursive(token, path);
        }

        return 0;
    }

    private static int DeleteDirectoryRecursive(Token token, string dirPath)
    {
        var count = 0;

        try
        {
            var files = Directory.GetFiles(dirPath);
            foreach (var file in files)
            {
                File.Delete(file);
                count++;
            }

            var dirs = Directory.GetDirectories(dirPath);
            foreach (var d in dirs)
            {
                count += DeleteDirectoryRecursive(token, d);
            }

            Directory.Delete(dirPath, false);
            count++;
        }
        catch (Exception)
        {
            throw new FileSystemError(token, "Could not remove path: " + dirPath);
        }

        return count;
    }

    public static bool IsScript(Token token, string path)
    {
        var extension = GetFileExtension(token, path);
        return Hayward.Settings.Extensions.Recognized.Contains(extension);
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

    public static List<Value> ReadLines(Token token, string filePath)
    {
        try
        {
            return File.ReadAllLines(filePath).Select(x => Value.CreateString(x)).ToList();
        }
        catch (Exception)
        {
            throw new FileReadError(token, filePath);
        }
    }

    public static List<Value> ReadBytes(Token token, string filePath)
    {
        try
        {
            return File.ReadAllBytes(filePath).Select(x => Value.CreateInteger(x)).ToList();
        }
        catch (Exception)
        {
            throw new FileReadError(token, filePath);
        }
    }

    public static bool AppendText(Token token, string path, string text)
    {
        try
        {
            File.AppendAllText(path, text);
            return true;
        }
        catch (Exception)
        {
            throw new FileSystemError(token, $"Could not write to file: {path}");
        }
    }

    public static bool WriteText(Token token, string path, string text)
    {
        try
        {
            File.WriteAllText(path, text);
            return true;
        }
        catch (Exception)
        {
            throw new FileSystemError(token, $"Could not write to file: {path}");
        }
    }

    public static bool WriteLine(Token token, string path, string text)
    {
        try
        {
            File.AppendAllText(path, text + Environment.NewLine);
            return true;
        }
        catch (Exception)
        {
            throw new FileSystemError(token, $"Could not write to file: {path}");
        }
    }

    public static bool WriteLines(Token token, string path, List<string> text)
    {
        try
        {
            File.AppendAllLines(path, text);
            return true;
        }
        catch (Exception)
        {
            throw new FileSystemError(token, $"Could not write to file: {path}");
        }
    }

    public static int WriteBytes(Token token, string path, List<Value> bytes)
    {
        try
        {
            File.WriteAllBytes(path, bytes.Select(x => (byte)x.GetInteger()).ToArray());
            return bytes.Count;
        }
        catch (Exception)
        {
            throw new FileSystemError(token, $"Could not write to file: {path}");
        }
    }

    public static string TryGetExtensionless(Token token, string path)
    {
        if (IsScript(token, path) && FileExists(token, path) && !DirectoryExists(token, path))
        {
            return path;
        }

        foreach (var ext in Hayward.Settings.Extensions.Recognized)
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