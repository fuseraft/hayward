# Packages

Use the `package` keyword to define a package. Use the `import` keyword to import a package.

Methods defined in a package can only be accessed using a fully qualified name.

```hayward
# Defining packages.
package greeter
  fn greet(name)
    println("Hello, ${name}.");
;

package excited_greeter
  fn greet(name)
    println("Hello, ${name}!");
;

# Importing packages.
export "greeter"
export "excited_greeter"

# Invoking their methods.
greeter::greet("World")
excited_greeter::greet("World")
```

The output will be:
```
Hello World.
Hello World!
```
