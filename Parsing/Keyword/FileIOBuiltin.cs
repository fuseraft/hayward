namespace citrus.Parsing.Keyword;

public static class FileIOBuiltin
{
    // File operations
    public const string AppendText = "__fileio_appendtext__";
    public const string CopyFile = "__fileio_copyfile__";
    public const string CopyR = "__fileio_copyr__";
    public const string Combine = "__fileio_combine__";
    public const string CreateFile = "__fileio_createfile__";
    public const string DeleteFile = "__fileio_deletefile__";
    public const string MoveFile = "__fileio_movefile__";
    public const string ReadFile = "__fileio_readfile__";
    public const string ReadLines = "__fileio_readlines__";
    public const string ReadBytes = "__fileio_readbytes__";
    public const string WriteLine = "__fileio_writeline__";
    public const string WriteText = "__fileio_writetext__";
    public const string WriteBytes = "__fileio_writebytes__";
    public const string FileExists = "__fileio_isfile__";
    public const string GetFileExtension = "__fileio_fileext__";
    public const string FileName = "__fileio_filename__";
    public const string FileSize = "__fileio_filesize__";
    public const string GetFilePath = "__fileio_filepath__";
    public const string GetFileAbsolutePath = "__fileio_fileabspath__";
    public const string GetFileAttributes = "__fileio_fileattrs__";
    public const string Glob = "__fileio_glob__";

    // Directory operations
    public const string ListDirectory = "__fileio_listdir__";
    public const string MakeDirectory = "__fileio_mkdir__";
    public const string MakeDirectoryP = "__fileio_mkdirp__";
    public const string RemoveDirectory = "__fileio_rmdir__";
    public const string RemoveDirectoryF = "__fileio_rmdirf__";
    public const string IsDirectory = "__fileio_isdir__";
    public const string ChangeDirectory = "__fileio_chdir__";
    public const string GetCurrentDirectory = "__fileio_cwd__";
    public const string TempDir = "__fileio_tmpdir__";

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

    private static readonly IReadOnlySet<TokenName> _names = Map.Values.ToHashSet();

    public static IReadOnlyDictionary<string, TokenName> Map => _map;

    public static bool IsBuiltin(TokenName name) => _names.Contains(name);
}
