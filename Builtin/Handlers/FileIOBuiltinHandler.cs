using citrus.Parsing;
using citrus.Parsing.Builtins;
using citrus.Tracing.Error;
using citrus.Typing;

namespace citrus.Builtin.Handlers;

public static class FileIOBuiltinHandler
{

    public static Value Execute(Token token, TokenName builtin, Value value, List<Value> args)
    {
        return builtin switch
        {
            TokenName.Builtin_FileIO_CreateFile => ExecuteCreateFile(token, args),
            TokenName.Builtin_FileIO_MakeDirectory => ExecuteMakeDirectory(token, args),
            TokenName.Builtin_FileIO_MakeDirectoryP => ExecuteMakeDirectoryP(token, args),
            TokenName.Builtin_FileIO_DeleteFile => ExecuteRemovePath(token, args),
            TokenName.Builtin_FileIO_RemoveDirectory => ExecuteRemovePath(token, args),
            TokenName.Builtin_FileIO_RemoveDirectoryF => ExecuteRemovePathF(token, args),
            TokenName.Builtin_FileIO_FileExists => ExecuteFileExists(token, args),
            TokenName.Builtin_FileIO_IsDirectory => ExecuteIsDirectory(token, args),
            TokenName.Builtin_FileIO_GetFileExtension => ExecuteGetFileExtension(token, args),
            TokenName.Builtin_FileIO_GetCurrentDirectory => ExecuteGetCurrentDirectory(token, args),
            TokenName.Builtin_FileIO_FileName => ExecuteGetFileName(token, args),
            TokenName.Builtin_FileIO_GetFilePath => ExecuteGetFilePath(token, args),
            TokenName.Builtin_FileIO_GetFileAbsolutePath => ExecuteGetFileAbsolutePath(token, args),
            TokenName.Builtin_FileIO_TempDir => ExecuteGetTempDirectory(token, args),
            TokenName.Builtin_FileIO_ChangeDirectory => ExecuteChangeDirectory(token, args),
            TokenName.Builtin_FileIO_CopyFile => ExecuteCopyFile(token, args),
            TokenName.Builtin_FileIO_CopyR => ExecuteCopyR(token, args),
            TokenName.Builtin_FileIO_MoveFile => ExecuteMoveFile(token, args),
            TokenName.Builtin_FileIO_ReadBytes => ExecuteReadBytes(token, args),
            TokenName.Builtin_FileIO_ReadFile => ExecuteReadFile(token, args),
            TokenName.Builtin_FileIO_ReadLines => ExecuteReadLines(token, args),
            TokenName.Builtin_FileIO_AppendText => ExecuteAppendText(token, args),
            TokenName.Builtin_FileIO_WriteBytes => ExecuteWriteBytes(token, args),
            TokenName.Builtin_FileIO_WriteText => ExecuteWriteText(token, args),
            TokenName.Builtin_FileIO_WriteLine => ExecuteWriteLine(token, args),
            TokenName.Builtin_FileIO_Glob => ExecuteGlob(token, args),
            TokenName.Builtin_FileIO_ListDirectory => ExecuteListDirectory(token, args),
            TokenName.Builtin_FileIO_FileSize => ExecuteGetFileSize(token, args),
            TokenName.Builtin_FileIO_Combine => ExecuteCombine(token, args),
            _ => throw new FunctionUndefinedError(token, token.Text),
        };
    }

    private static Value ExecuteCombine(Token token, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.Combine);
        }

        if (!args[0].IsList())
        {
            throw new InvalidOperationError(token, "Expected a list.");
        }

        List<string> paths = [];

        foreach (var item in args[0].GetList())
        {
            if (!item.IsString())
            {
                throw new InvalidOperationError(token, "Expected a list of strings.");
            }

            paths.Add(item.GetString());
        }

        var path = FileUtil.CombinePath(token, paths);
        return Value.CreateList(path);
    }

    private static Value ExecuteGetFileSize(Token token, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.FileSize);
        }

        if (!args[0].IsString())
        {
            throw new InvalidOperationError(token, "Expected a string.");
        }

        var path = args[0].GetString();

        return Value.CreateInteger(FileUtil.GetFileSize(token, path));
    }

    private static Value ExecuteListDirectory(Token token, List<Value> args)
    {
        if (args.Count != 1 && args.Count != 2)
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.ListDirectory);
        }

        if (!args[0].IsString())
        {
            throw new InvalidOperationError(token, "Expected a string.");
        }

        var path = args[0].GetString();
        var recursive = false;

        if (args.Count == 2)
        {
            if (!args[1].IsBoolean())
            {
                throw new InvalidOperationError(token, "Expected a boolean.");            
            }

            recursive = args[1].GetBoolean();
        }

        var contents = FileUtil.ListDirectory(token, path, recursive);
        return Value.CreateList(contents);
    }

    private static Value ExecuteGlob(Token token, List<Value> args)
    {
        if (args.Count != 2 && args.Count != 3)
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.Glob);
        }

        if (!args[0].IsString())
        {
            throw new InvalidOperationError(token, "Expected a string.");
        }

        if (!args[1].IsList() || args[1].GetList().Count == 0)
        {
            throw new InvalidOperationError(token, "Expected a non-empty list.");
        }

        if (args.Count == 3 && !args[2].IsList())
        {
            throw new InvalidOperationError(token, "Expected a list.");
        }

        List<string> includePatterns = [];
        List<string> excludePatterns = [];
        var path = args[0].GetString();

        foreach (var item in args[1].GetList())
        {
            if (!item.IsString())
            {
                throw new InvalidOperationError(token, "Expected a list of strings.");
            }

            includePatterns.Add(item.GetString());
        }

        if (args.Count == 3)
        {
            foreach (var item in args[2].GetList())
            {
                if (!item.IsString())
                {
                    throw new InvalidOperationError(token, "Expected a list of strings.");
                }

                excludePatterns.Add(item.GetString());
            }
        }

        var files = FileUtil.Glob(token, path, includePatterns, excludePatterns);
        return Value.CreateList(files);
    }

    private static Value ExecuteAppendText(Token token, List<Value> args)
    {
        if (args.Count != 2 && !args[0].IsString() && !args[1].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.AppendText);
        }

        var path = args[0].GetString();
        var text = args[1].GetString();

        return Value.CreateBoolean(FileUtil.AppendText(token, path, text));
    }

    private static Value ExecuteWriteBytes(Token token, List<Value> args)
    {
        if (args.Count != 2 && !args[0].IsString() && !args[1].IsList())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.WriteBytes);
        }

        var path = args[0].GetString();
        var bytes = args[1].GetList();

        return Value.CreateBoolean(FileUtil.WriteBytes(token, path, bytes));
    }

    private static Value ExecuteWriteText(Token token, List<Value> args)
    {
        if (args.Count != 2 && !args[0].IsString() && !args[1].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.WriteText);
        }

        var path = args[0].GetString();
        var text = args[1].GetString();

        return Value.CreateBoolean(FileUtil.WriteText(token, path, text));
    }

    private static Value ExecuteWriteLine(Token token, List<Value> args)
    {
        if (args.Count != 2 && !args[0].IsString() && !args[1].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.WriteLine);
        }

        var path = args[0].GetString();
        var text = args[1].GetString();

        return Value.CreateBoolean(FileUtil.WriteLine(token, path, text));
    }

    private static Value ExecuteReadBytes(Token token, List<Value> args)
    {
        if (args.Count != 1 && !args[0].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.ReadBytes);
        }

        var path = args[0].GetString();
        return Value.CreateList(FileUtil.ReadBytes(token, path));
    }

    private static Value ExecuteReadFile(Token token, List<Value> args)
    {
        if (args.Count != 1 && !args[0].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.ReadFile);
        }

        var path = args[0].GetString();
        return Value.CreateList(FileUtil.ReadFile(token, path));
    }

    private static Value ExecuteReadLines(Token token, List<Value> args)
    {
        if (args.Count != 1 && !args[0].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.ReadLines);
        }

        var path = args[0].GetString();
        return Value.CreateList(FileUtil.ReadLines(token, path));
    }

    private static Value ExecuteMoveFile(Token token, List<Value> args)
    {
        if (args.Count != 2 && !args[0].IsString() && !args[1].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.MoveFile);
        }

        var src = args[0].GetString();
        var dst = args[1].GetString();

        return Value.CreateBoolean(FileUtil.MoveFile(token, src, dst));
    }

    private static Value ExecuteCopyFile(Token token, List<Value> args)
    {
        if (args.Count != 2 && !args[0].IsString() && !args[1].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.CopyFile);
        }

        var src = args[0].GetString();
        var dst = args[1].GetString();

        return Value.CreateBoolean(FileUtil.CopyFile(token, src, dst));
    }

    private static Value ExecuteCopyR(Token token, List<Value> args)
    {
        if (args.Count != 2 && !args[0].IsString() && !args[1].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.CopyR);
        }

        var src = args[0].GetString();
        var dst = args[1].GetString();

        return Value.CreateBoolean(FileUtil.CopyDirectoryRecursive(token, src, dst));
    }

    private static Value ExecuteChangeDirectory(Token token, List<Value> args)
    {
        if (args.Count != 1 && !args[0].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.ChangeDirectory);
        }

        var path = args[0].GetString();
        return Value.CreateBoolean(FileUtil.ChangeDirectory(token, path));
    }

    private static Value ExecuteGetTempDirectory(Token token, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.TempDir);
        }

        return Value.CreateString(FileUtil.GetTempDirectory());
    }

    private static Value ExecuteMakeDirectory(Token token, List<Value> args)
    {
        if (args.Count != 1 && !args[0].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.MakeDirectory);
        }

        var fileName = args[0].GetString();
        return Value.CreateBoolean(FileUtil.MakeDirectory(token, fileName));
    }

    private static Value ExecuteMakeDirectoryP(Token token, List<Value> args)
    {
        if (args.Count != 1 && !args[0].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.MakeDirectoryP);
        }

        var fileName = args[0].GetString();
        return Value.CreateBoolean(FileUtil.MakeDirectory(token, fileName));
    }
    
    private static Value ExecuteRemovePath(Token token, List<Value> args)
    {
        if (args.Count != 1 && !args[0].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.RemoveDirectory);
        }

        var fileName = args[0].GetString();
        return Value.CreateBoolean(FileUtil.RemovePath(token, fileName));
    }
    
    private static Value ExecuteRemovePathF(Token token, List<Value> args)
    {
        if (args.Count != 1 && !args[0].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.RemoveDirectoryF);
        }

        var fileName = args[0].GetString();
        return Value.CreateInteger(FileUtil.RemovePathRecursive(token, fileName));
    }

    private static Value ExecuteGetFileExtension(Token token, List<Value> args)
    {
        if (args.Count != 1 && !args[0].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.GetFileExtension);
        }

        var fileName = args[0].GetString();
        return Value.CreateString(FileUtil.GetFileExtension(token, fileName));
    }

    private static Value ExecuteGetCurrentDirectory(Token token, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.CreateFile);
        }

        return Value.CreateString(FileUtil.GetCurrentDirectory());
    }

    private static Value ExecuteGetFileName(Token token, List<Value> args)
    {
        if (args.Count != 1 && !args[0].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.FileName);
        }

        var fileName = args[0].GetString();
        return Value.CreateString(FileUtil.GetFileName(token, fileName));
    }

    private static Value ExecuteGetFilePath(Token token, List<Value> args)
    {
        if (args.Count != 1 && !args[0].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.GetFilePath);
        }

        var fileName = args[0].GetString();
        return Value.CreateString(FileUtil.GetParentPath(token, fileName));
    }

    private static Value ExecuteGetFileAbsolutePath(Token token, List<Value> args)
    {
        if (args.Count != 1 && !args[0].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.GetFileAbsolutePath);
        }

        var fileName = args[0].GetString();
        return Value.CreateString(FileUtil.GetAbsolutePath(token, fileName));
    }
            
    private static Value ExecuteCreateFile(Token token, List<Value> args)
    {
        if (args.Count != 1 && !args[0].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.CreateFile);
        }

        var fileName = args[0].GetString();
        return Value.CreateBoolean(FileUtil.CreateFile(token, fileName));
    }

    private static Value ExecuteFileExists(Token token, List<Value> args)
    {
        if (args.Count != 1 && !args[0].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.FileExists);
        }

        var fileName = args[0].GetString();
        return Value.CreateBoolean(FileUtil.FileExists(token, fileName));
    }

    private static Value ExecuteIsDirectory(Token token, List<Value> args)
    {
        if (args.Count != 1 && !args[0].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.IsDirectory);
        }

        var fileName = args[0].GetString();
        return Value.CreateBoolean(FileUtil.DirectoryExists(token, fileName));
    }
}