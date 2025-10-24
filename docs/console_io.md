# Console I/O

Hayward offers the following constructs to work with console I/O:
- `input(msg)`: Read input from the console.
- `print`: Print text to the console.
- `println`: Print a line of text to the console.

For file I/O, please see [fio](lib/fio.md).

### `input()`

To read input from the console, use the `input()` builtin.

```hayward
print("Enter your favorite fruit: ")
fruit = input()
# fruit contains "kiwi"

println("Your favorite fruit is ${fruit}")
# prints: Your favorite fruit is kiwi
```

You can also pass a string to print as a prompt.

```hayward
choice = ""
while !"ab".chars().contains(choice) do
  choice = input("(a)ttack or (b)lock: ")
end
```

### `print`

To print text to the console, use the `print` keyword.

```hayward
print("Hello")
print(", World!") # prints: Hello, World!"
```

### `println`

To print a line of text to the console, use the `println` keyword.

```hayward
println("Do you like programming?")
println("Let's build something amazing!")

# prints: 
# Do you like programming?
# Let's build something amazing!
```