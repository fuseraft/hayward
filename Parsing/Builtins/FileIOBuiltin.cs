namespace citrus.Parsing.Builtins;

public static class FileIOBuiltin
{
    // File operations
    public const string AppendText = "__appendtext__";
    public const string CopyFile = "__copyfile__";
    public const string CopyR = "__copyr__";
    public const string Combine = "__combine__";
    public const string CreateFile = "__createfile__";
    public const string DeleteFile = "__deletefile__";
    public const string MoveFile = "__movefile__";
    public const string ReadFile = "__readfile__";
    public const string ReadLines = "__readlines__";
    public const string ReadBytes = "__readbytes__";
    public const string WriteLine = "__writeline__";
    public const string WriteText = "__writetext__";
    public const string WriteBytes = "__writebytes__";
    public const string FileExists = "__isfile__";
    public const string GetFileExtension = "__fileext__";
    public const string FileName = "__filename__";
    public const string FileSize = "__filesize__";
    public const string GetFilePath = "__filepath__";
    public const string GetFileAbsolutePath = "__fileabspath__";
    public const string GetFileAttributes = "__fileattrs__";
    public const string Glob = "__glob__";

    // Directory operations
    public const string ListDirectory = "__listdir__";
    public const string MakeDirectory = "__mkdir__";
    public const string MakeDirectoryP = "__mkdirp__";
    public const string RemoveDirectory = "__rmdir__";
    public const string RemoveDirectoryF = "__rmdirf__";
    public const string IsDirectory = "__isdir__";
    public const string ChangeDirectory = "__chdir__";
    public const string GetCurrentDirectory = "__cwd__";
    public const string TempDir = "__tmpdir__";

    private static readonly IReadOnlyDictionary<string, TokenName> _map
        = new Dictionary<string, TokenName>
        {
            { AppendText,          TokenName.Builtin_FileIO_AppendText },
            { CopyFile,            TokenName.Builtin_FileIO_CopyFile },
            { CopyR,               TokenName.Builtin_FileIO_CopyR },
            { Combine,             TokenName.Builtin_FileIO_Combine },
            { CreateFile,          TokenName.Builtin_FileIO_CreateFile },
            { DeleteFile,          TokenName.Builtin_FileIO_DeleteFile },
            { MoveFile,            TokenName.Builtin_FileIO_MoveFile },
            { ReadFile,            TokenName.Builtin_FileIO_ReadFile },
            { ReadLines,           TokenName.Builtin_FileIO_ReadLines },
            { ReadBytes,           TokenName.Builtin_FileIO_ReadBytes },
            { WriteLine,           TokenName.Builtin_FileIO_WriteLine },
            { WriteText,           TokenName.Builtin_FileIO_WriteText },
            { WriteBytes,          TokenName.Builtin_FileIO_WriteBytes },
            { FileExists,          TokenName.Builtin_FileIO_FileExists },
            { GetFileExtension,    TokenName.Builtin_FileIO_GetFileExtension },
            { FileName,            TokenName.Builtin_FileIO_FileName },
            { FileSize,            TokenName.Builtin_FileIO_FileSize },
            { GetFilePath,         TokenName.Builtin_FileIO_GetFilePath },
            { GetFileAbsolutePath, TokenName.Builtin_FileIO_GetFileAbsolutePath },
            { GetFileAttributes,   TokenName.Builtin_FileIO_GetFileAttributes },
            { Glob,                TokenName.Builtin_FileIO_Glob },
            { ListDirectory,       TokenName.Builtin_FileIO_ListDirectory },
            { MakeDirectory,       TokenName.Builtin_FileIO_MakeDirectory },
            { MakeDirectoryP,      TokenName.Builtin_FileIO_MakeDirectoryP },
            { RemoveDirectory,     TokenName.Builtin_FileIO_RemoveDirectory },
            { RemoveDirectoryF,    TokenName.Builtin_FileIO_RemoveDirectoryF },
            { IsDirectory,         TokenName.Builtin_FileIO_IsDirectory },
            { ChangeDirectory,     TokenName.Builtin_FileIO_ChangeDirectory },
            { GetCurrentDirectory, TokenName.Builtin_FileIO_GetCurrentDirectory },
            { TempDir,             TokenName.Builtin_FileIO_TempDir },
        };

    public static IReadOnlyDictionary<string, TokenName> Map => _map;
}
