# Functions and Methods

Use the `fn` keyword to define a function.

Use the `def` keyword to define a method (a function of a [`struct`](structs.md)). 

The distinction between a function and a method in Hayward is purely semantic.

```hayward
fn greet(name = "Hayward")
  println("Hello, ${name}!");

greet("world") # prints: Hello, world!
greet()        # prints: Hello, Hayward!
```

### Return Value

Use the `return` keyword to return a value from a method, or to exit a method early.

```hayward
fn get_greeting(name)
  return "Hello, ${name}";

greeting = get_greeting("World!")

println(greeting)
```

### Optional Parameters

```hayward
fn say(msg = "Hello, World!")
  println(msg);

say()       # prints: Hello, World!
say("Hey!") # prints: Hey!

fn configure(data, config = {})
  for key in config.keys() do
    data[key] = config[key];

  return data;

data = configure({ "name": "Scott" })
println(data) # prints: {"name": "Scott"}

data = configure({ "name": "Scott" }, { "favorite_os": "Fedora" })
println(data) # prints: {"name": "Scott", "favorite_os": "Fedora"}
```

### Scope

You can access all global variables from within a method.

```hayward
counter = 0

fn uptick()
  counter += 1;

i = 1
while i <= 5 do
  uptick()
  i += 1;

# 5
println(counter)
```