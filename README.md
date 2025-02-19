# Hayward 🥝

A dynamically-typed object-oriented scripting language that runs on .NET.

## Getting Started

Read the [documentation](docs/README.md). 

Clone the repository:
```bash
git clone https://github.com/fuseraft/hayward.git
```

Build Hayward:
```bash
./build.sh
```

Run the test suite:
```bash
./bin/hayward scripts/test
```

## VS Code Extension

For syntax highlighting and code snippets in Visual Studio Code, install the [VS Code extension](https://marketplace.visualstudio.com/items?itemName=fuseraft.kiwi-lang).

## 

```hayward
/#
This simple markdown generator reads all `.hayward` files found within the current 
directory and its subdirectories, and generates a markdown file called `output.md` 
containing the contents of each file.
#/

fn generate_markdown(output_name: string = "output.md",
                     ext:         string = ".cs",
                     lang:        string = "csharp")
  try
    # get the absolute path to the output file (in the current directory)
    var (output_path: string = fio::abspath("./${output_name}"))
    
    # this list will contain the markdown content
    var (markdown: list = [])
    
    # remove the file if it exists
    fio::remove(output_path)

    # loop through each file with a given extension
    for path in fio::glob("./", ["./**/*${ext}"]) do
      # get the filename and read the text from the file
      var (filename:  string = fio::filename(path),
           content:   string = fio::read(path))

      # create markdown content for the current file
      markdown.push([
        "## ${filename}", 
        "```${lang}", 
        content, 
        "```"
      ]);

    # write the markdown content to the output file
    fio::writeln(output_path, markdown.flatten())

    # throw an error if we could not generate the file
    throw "could not generate ${output_path}" 
      when !fio::exists(output_path)
    
    println "generated ${output_path}"
  catch (err)
    println "an error occurred: ${err}";
;

generate_markdown("hayward-files.md", ".hayward", "hayward")
```

## Contributing

Contributions are highly appreciated! Here’s how to get involved:

1. **Join the Discussion**: Join the community on [Discord](https://discord.gg/9PW3857Bxs).
2. **Fork the Repository**: Fork Hayward on GitHub.
3. **Clone the Repository**: Clone your forked copy to your machine.
4. **Set Up Your Environment**: Follow the steps in "Getting Started."
5. **Make Changes**: Implement new features or fix issues.
6. **Test Your Changes**: Run all tests to ensure stability.
7. **Submit a Pull Request**: Submit your changes for review.

For more details, please refer to [CONTRIBUTING.md](CONTRIBUTING.md).

## License

This project is licensed under the [MIT License](LICENSE).