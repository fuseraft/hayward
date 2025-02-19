using hayward.Parsing;
using hayward.Parsing.Keyword;
using hayward.Tracing.Error;
using hayward.Typing;

namespace hayward.Runtime.Builtin.Handlers;

public static class FileIOBuiltinHandler
{

    public static Value Execute(Token token, TokenName builtin, List<Value> args)
    {
        return builtin switch
        {
            TokenName.Builtin_FileIO_CreateFile => CreateFile(token, args),
            TokenName.Builtin_FileIO_MakeDirectory => MakeDirectory(token, args),
            TokenName.Builtin_FileIO_MakeDirectoryP => MakeDirectoryP(token, args),
            TokenName.Builtin_FileIO_DeleteFile => RemovePath(token, args),
            TokenName.Builtin_FileIO_RemoveDirectory => RemovePath(token, args),
            TokenName.Builtin_FileIO_RemoveDirectoryF => RemovePathF(token, args),
            TokenName.Builtin_FileIO_FileExists => FileExists(token, args),
            TokenName.Builtin_FileIO_IsDirectory => IsDirectory(token, args),
            TokenName.Builtin_FileIO_GetFileExtension => GetFileExtension(token, args),
            TokenName.Builtin_FileIO_GetCurrentDirectory => GetCurrentDirectory(token, args),
            TokenName.Builtin_FileIO_FileName => GetFileName(token, args),
            TokenName.Builtin_FileIO_GetFilePath => GetFilePath(token, args),
            TokenName.Builtin_FileIO_GetFileAbsolutePath => GetFileAbsolutePath(token, args),
            TokenName.Builtin_FileIO_TempDir => GetTempDirectory(token, args),
            TokenName.Builtin_FileIO_ChangeDirectory => ChangeDirectory(token, args),
            TokenName.Builtin_FileIO_CopyFile => CopyFile(token, args),
            TokenName.Builtin_FileIO_CopyR => CopyR(token, args),
            TokenName.Builtin_FileIO_MoveFile => MoveFile(token, args),
            TokenName.Builtin_FileIO_ReadBytes => ReadBytes(token, args),
            TokenName.Builtin_FileIO_ReadFile => ReadFile(token, args),
            TokenName.Builtin_FileIO_ReadLines => ReadLines(token, args),
            TokenName.Builtin_FileIO_AppendText => AppendText(token, args),
            TokenName.Builtin_FileIO_WriteBytes => WriteBytes(token, args),
            TokenName.Builtin_FileIO_WriteText => WriteText(token, args),
            TokenName.Builtin_FileIO_WriteLine => WriteLine(token, args),
            TokenName.Builtin_FileIO_Glob => Glob(token, args),
            TokenName.Builtin_FileIO_ListDirectory => ListDirectory(token, args),
            TokenName.Builtin_FileIO_FileSize => GetFileSize(token, args),
            TokenName.Builtin_FileIO_Combine => Combine(token, args),
            _ => throw new FunctionUndefinedError(token, token.Text),
        };
    }

    private static Value Combine(Token token, List<Value> args)
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
        return Value.CreateString(path);
    }

    private static Value GetFileSize(Token token, List<Value> args)
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

    private static Value ListDirectory(Token token, List<Value> args)
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

    private static Value Glob(Token token, List<Value> args)
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

    private static Value AppendText(Token token, List<Value> args)
    {
        if (args.Count != 2 && !args[0].IsString() && !args[1].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.AppendText);
        }

        var path = args[0].GetString();
        var text = args[1].GetString();

        return Value.CreateBoolean(FileUtil.AppendText(token, path, text));
    }

    private static Value WriteBytes(Token token, List<Value> args)
    {
        if (args.Count != 2 && !args[0].IsString() && !args[1].IsList())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.WriteBytes);
        }

        var path = args[0].GetString();
        var bytes = args[1].GetList();

        return Value.CreateBoolean(FileUtil.WriteBytes(token, path, bytes));
    }

    private static Value WriteText(Token token, List<Value> args)
    {
        if (args.Count != 2 && !args[0].IsString() && !args[1].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.WriteText);
        }

        var path = args[0].GetString();
        var text = args[1].GetString();

        return Value.CreateBoolean(FileUtil.WriteText(token, path, text));
    }

    private static Value WriteLine(Token token, List<Value> args)
    {
        if (args.Count != 2 && !args[0].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.WriteLine);
        }

        var path = args[0].GetString();

        if (args[1].IsString())
        {
            var text = args[1].GetString();
            return Value.CreateBoolean(FileUtil.WriteLine(token, path, text));
        }
        else if (args[1].IsList())
        {
            List<string> text = [];
            foreach (var item in args[1].GetList())
            {
                text.Add(Serializer.Serialize(item));
            }
            return Value.CreateBoolean(FileUtil.WriteLines(token, path, text));
        }

        throw new InvalidOperationError(token, "Expected a string or a list.");
    }

    private static Value ReadBytes(Token token, List<Value> args)
    {
        if (args.Count != 1 && !args[0].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.ReadBytes);
        }

        var path = args[0].GetString();
        return Value.CreateList(FileUtil.ReadBytes(token, path));
    }

    private static Value ReadFile(Token token, List<Value> args)
    {
        if (args.Count != 1 && !args[0].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.ReadFile);
        }

        var path = args[0].GetString();
        return Value.CreateString(FileUtil.ReadFile(token, path));
    }

    private static Value ReadLines(Token token, List<Value> args)
    {
        if (args.Count != 1 && !args[0].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.ReadLines);
        }

        var path = args[0].GetString();
        return Value.CreateList(FileUtil.ReadLines(token, path));
    }

    private static Value MoveFile(Token token, List<Value> args)
    {
        if (args.Count != 2 && !args[0].IsString() && !args[1].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.MoveFile);
        }

        var src = args[0].GetString();
        var dst = args[1].GetString();

        return Value.CreateBoolean(FileUtil.MoveFile(token, src, dst));
    }

    private static Value CopyFile(Token token, List<Value> args)
    {
        if (args.Count != 2 && !args[0].IsString() && !args[1].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.CopyFile);
        }

        var src = args[0].GetString();
        var dst = args[1].GetString();

        return Value.CreateBoolean(FileUtil.CopyFile(token, src, dst));
    }

    private static Value CopyR(Token token, List<Value> args)
    {
        if (args.Count != 2 && !args[0].IsString() && !args[1].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.CopyR);
        }

        var src = args[0].GetString();
        var dst = args[1].GetString();

        return Value.CreateBoolean(FileUtil.CopyDirectoryRecursive(token, src, dst));
    }

    private static Value ChangeDirectory(Token token, List<Value> args)
    {
        if (args.Count != 1 && !args[0].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.ChangeDirectory);
        }

        var path = args[0].GetString();
        return Value.CreateBoolean(FileUtil.ChangeDirectory(token, path));
    }

    private static Value GetTempDirectory(Token token, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.TempDir);
        }

        return Value.CreateString(FileUtil.GetTempDirectory());
    }

    private static Value MakeDirectory(Token token, List<Value> args)
    {
        if (args.Count != 1 && !args[0].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.MakeDirectory);
        }

        var fileName = args[0].GetString();
        return Value.CreateBoolean(FileUtil.MakeDirectory(token, fileName));
    }

    private static Value MakeDirectoryP(Token token, List<Value> args)
    {
        if (args.Count != 1 && !args[0].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.MakeDirectoryP);
        }

        var fileName = args[0].GetString();
        return Value.CreateBoolean(FileUtil.MakeDirectory(token, fileName));
    }

    private static Value RemovePath(Token token, List<Value> args)
    {
        if (args.Count != 1 && !args[0].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.RemoveDirectory);
        }

        var fileName = args[0].GetString();
        return Value.CreateBoolean(FileUtil.RemovePath(token, fileName));
    }

    private static Value RemovePathF(Token token, List<Value> args)
    {
        if (args.Count != 1 && !args[0].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.RemoveDirectoryF);
        }

        var fileName = args[0].GetString();
        return Value.CreateInteger(FileUtil.RemovePathRecursive(token, fileName));
    }

    private static Value GetFileExtension(Token token, List<Value> args)
    {
        if (args.Count != 1 && !args[0].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.GetFileExtension);
        }

        var fileName = args[0].GetString();
        return Value.CreateString(FileUtil.GetFileExtension(token, fileName));
    }

    private static Value GetCurrentDirectory(Token token, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.CreateFile);
        }

        return Value.CreateString(FileUtil.GetCurrentDirectory());
    }

    private static Value GetFileName(Token token, List<Value> args)
    {
        if (args.Count != 1 && !args[0].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.FileName);
        }

        var fileName = args[0].GetString();
        return Value.CreateString(FileUtil.GetFileName(token, fileName));
    }

    private static Value GetFilePath(Token token, List<Value> args)
    {
        if (args.Count != 1 && !args[0].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.GetFilePath);
        }

        var fileName = args[0].GetString();
        return Value.CreateString(FileUtil.GetParentPath(token, fileName));
    }

    private static Value GetFileAbsolutePath(Token token, List<Value> args)
    {
        if (args.Count != 1 && !args[0].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.GetFileAbsolutePath);
        }

        var fileName = args[0].GetString();
        return Value.CreateString(FileUtil.GetAbsolutePath(token, fileName));
    }

    private static Value CreateFile(Token token, List<Value> args)
    {
        if (args.Count != 1 && !args[0].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.CreateFile);
        }

        var fileName = args[0].GetString();
        return Value.CreateBoolean(FileUtil.CreateFile(token, fileName));
    }

    private static Value FileExists(Token token, List<Value> args)
    {
        if (args.Count != 1 && !args[0].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.FileExists);
        }

        var fileName = args[0].GetString();
        return Value.CreateBoolean(FileUtil.FileExists(token, fileName));
    }

    private static Value IsDirectory(Token token, List<Value> args)
    {
        if (args.Count != 1 && !args[0].IsString())
        {
            throw new ParameterCountMismatchError(token, FileIOBuiltin.IsDirectory);
        }

        var fileName = args[0].GetString();
        return Value.CreateBoolean(FileUtil.DirectoryExists(token, fileName));
    }
}