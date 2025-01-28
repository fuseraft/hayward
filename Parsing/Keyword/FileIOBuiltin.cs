namespace hayward.Parsing.Keyword;

public static class FileIOBuiltin
{
    // File operations
    public const string AppendText = "__fio_appendtext__";
    public const string CopyFile = "__fio_copyfile__";
    public const string CopyR = "__fio_copyr__";
    public const string Combine = "__fio_combine__";
    public const string CreateFile = "__fio_createfile__";
    public const string DeleteFile = "__fio_deletefile__";
    public const string MoveFile = "__fio_movefile__";
    public const string ReadFile = "__fio_readfile__";
    public const string ReadLines = "__fio_readlines__";
    public const string ReadBytes = "__fio_readbytes__";
    public const string WriteLine = "__fio_writeline__";
    public const string WriteText = "__fio_writetext__";
    public const string WriteBytes = "__fio_writebytes__";
    public const string FileExists = "__fio_isfile__";
    public const string GetFileExtension = "__fio_fileext__";
    public const string FileName = "__fio_filename__";
    public const string FileSize = "__fio_filesize__";
    public const string GetFilePath = "__fio_filepath__";
    public const string GetFileAbsolutePath = "__fio_fileabspath__";
    public const string GetFileAttributes = "__fio_fileattrs__";
    public const string Glob = "__fio_glob__";

    // Directory operations
    public const string ListDirectory = "__fio_listdir__";
    public const string MakeDirectory = "__fio_mkdir__";
    public const string MakeDirectoryP = "__fio_mkdirp__";
    public const string RemoveDirectory = "__fio_rmdir__";
    public const string RemoveDirectoryF = "__fio_rmdirf__";
    public const string IsDirectory = "__fio_isdir__";
    public const string ChangeDirectory = "__fio_chdir__";
    public const string GetCurrentDirectory = "__fio_cwd__";
    public const string TempDir = "__fio_tmpdir__";

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
