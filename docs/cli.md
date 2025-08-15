# Command-Line Interface

The Hayward CLI provides an interface for working with Hayward files.

### Basic Usage

```
hayward [--flags] <script|args>
```

### Table of Contents
- [Options](#options)
  - [`-h`, `--help`](#-h---help)
  - [`-v`, `--version`](#-v---version)
  - [`-s`, `--settings`](#-s---settings)
  - [`-n`, `--new <file_path>`](#-n---new-file_path)
  - [`-i`, `--interactive`](#-i---interactive)
  - [`-a`, `--ast <input_file_path>`](#-a---ast-input_file_path)
  - [`-t`, `--tokenize <input_file_path>`](#-t---tokenize-input_file_path)
  - [`-ns`, `--no-stdlib`](#-ns---no-stdlib)
  - [`-sm`, `--safemode`](#-sm---safemode)
  - [`-<key>=<value>`](#-keyvalue)

## Options

### `-h`, `--help`

Displays help information with a summary of all available commands and their purposes.

```
hayward -h
```

### `-v`, `--version`

Prints the current version.

```
hayward -v
```

### `-s`, `--settings`

Prints the interpreter settings (for debugging).

```
hayward -s
```

### `-i`, `--interactive`

Starts an interactive Read-Evaluate-Print Loop (REPL) where you can type and execute Hayward code directly in the terminal. 

To learn more about the REPL, please see [REPL](repl.md).


```
hayward -i
```

### `-a`, `--ast <input_file_path>`

Prints the abstract syntax tree of the input file.

```
hayward -a filename         # Prints AST of the file.
```

### `-t`, `--tokenize <input_file_path>`

Tokenizes a file with the Hayward lexer and prints the results to the standard output stream.

```
hayward -t filename         # Prints tokens by line from the file
```

### `-n`, `--new <file_path>`

Creates a new file with the `.hayward` extension. If you don't include the extension in `<file_path>`, it will be appended automatically.

```
hayward -n filename         # Creates ./filename.hayward
hayward -n ../app           # Creates ../app.hayward in the parent directory
hayward -n ./somedir/app    # Creates ./somedir/app.hayward in the specified subdirectory
```

Note: If a file with the same name already exists, the CLI will notify you to prevent accidental overwriting.<br><br>

### `-sm`, `--safemode`

Runs Hayward in safe mode. This disables access to web, filesystem, and system shell functionality.

```
hayward -t filename         # Prints tokens from the file in a minified way.
```

### `-ns`, `--no-stdlib`

Runs Hayward without loading its standard library.

```
hayward -ns filename.k        # Runs `filename.k` without loading the Hayward standard library.
```

### `-<key>=<value>`

Sets a specific argument as a key-value pair, which can be used for various configuration purposes or to pass parameters into scripts.

```
hayward myapp -theme=dark
```