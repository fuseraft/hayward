# REPL

### Introduction to the REPL

To use the REPL, run Hayward without passing any arguments. 

Hayward will print its current version (at the time of writing this, the current version is 1.3), provide some instructions and then a prompt line beginning with **`> `**.

```
$ kiwi
Hayward v2.0.4 REPL

Use `go` to execute, `exit` to exit the REPL.

>   
```

The Hayward REPL enqueues code as you enter it. It also keeps track of all lines you enter so that you can replay them.

Type some Hayward code into the REPL prompt and press <kbd>Enter</kbd>.

```
> println("Hello, World!")
```

When you are ready to execute, type `go` and press <kbd>Enter</kbd>. Hayward will execute the code in the queue and then reset the queue.

```
> println("Hello, World!")
> go
Hello, World!
> 
```

### Keyboard Controls

You can use the <kbd>&#8592;</kbd> and <kbd>&#8594;</kbd> arrow keys to navigate to different parts of the line for inline editing of the code before submitting to the REPL queue.

You can use the <kbd>&#8593;</kbd> and <kbd>&#8595;</kbd> arrow keys to cycle through the list of lines entered.