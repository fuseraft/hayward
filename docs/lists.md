# Lists

Lists are array-like objects that expand as needed and can contain values of multiple types.

# Table of Contents
- [Builtins](#Builtins)
- [Defining a `List`](#defining-a-list)
- [Defining a `List` with a `Range`](#defining-a-list-with-a-range)
- [Accessing Elements](#accessing-list-elements)
- [Adding Elements](#adding-elements-to-a-list)
- [Removing Elements](#removing-elements-from-a-list)
- [Filtering a `List`](#filtering-a-list)
- [Iterating a `List`](#iterating-a-list)
- [An Example](#an-example)

### Builtins

For documentation on `List` builtins, take a look at the [`List` builtins](builtins.md#list-builtins).

### Defining a List

```hayward
myList = ["Hayward", "is", "fun!"]
anotherList = [1, 2, 3, "four", "five", 6]
```

### Defining a List with a Range

You can define a list using a range.

```hayward
# This is equivalent to: numbers = [1, 2, 3, 4, 5]
numbers = [1..5] 
```

### Accessing List Elements

Bracket notation can be used to access elements by index, starting from 0.

```hayward
println("${myList[0]}")  # Outputs: Hayward
println(myList[2])       # Outputs: fun!
```

### Adding Elements to a List

```hayward
myList = []
anotherList = [1, 2]

myList.push("Hello, Hayward!")
myList.push(anotherList)

# myList now contains ["Hello, Hayward!", [1, 2]]
```

### Removing Elements from a List

You can use the `delete` keyword to remove an element of a list by index.

```hayward
list = "abcdef".chars()
delete list[3]
println(list) # prints: ["a", "b", "c", "e", "f"]
```

### Filtering a List

You can use the `.filter(lambda)` builtin to filter a list based on a condition.

```hayward
list = ["kiwi", "mango", "banana"]
println(list.filter(with (item) do return item.contains("i");))
# prints: ["kiwi"]
```

### Iterating a List

Use the `for` keyword to iterate a list.

```hayward
# Iterate the values in the list.
for item in myList do
  println("${item}");

# Iterate the values in the list, with an index.
for item, index in myList do
  println("Item ${index}: ${item}");
```

### An Example

```hayward
fn sieve_of_eratosthenes(limit)
  isPrime = []
  for i in [0..limit] do
     isPrime.push(true);

  isPrime[0] = false
  isPrime[1] = false

  p = 2

  while p * p <= limit do
    # If p is not changed, then it is a prime
    if isPrime[p]
      # Update all multiples of p
      multiple = p * 2
      while multiple <= limit do
        isPrime[multiple] = false
        multiple += p;
    ;

    p += 1;

  # Collect all prime numbers
  primes = []
  for i in [0..limit] do
    if isPrime[i]
      primes.push(i);
  ;

  return primes;

for prime, index in sieve_of_eratosthenes(30) do
  println("${index}:\t${prime}");
```