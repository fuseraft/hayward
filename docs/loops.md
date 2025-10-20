# Loops

Hayward supports the following kinds of loops. You can use the `break` keyword to exit a loop. You can use the `next` keyword to skip to the next iteration.

## Table of Contents
1. [`repeat`](#repeat)
2. [`while`](#while)
3. [`for`](#for)

## `repeat`

The `repeat`-loop is used to loop `n` number of times where `n` is a positive non-zero integer.

```hayward
/#
# add 1 to `i` ten times, then print the value of `i`.
#/

i = 0

repeat 10 do
  i += 1;

print i

# output: 10
```

Sometimes you need to know how many times a loop has executed. 

You can use the `as` keyword to specify an *iterator variable* which stores the number of times a loop has executed.

```hayward
/#
# repeat 10 times, using `i` as the iterator variable.
# print the value of `i` followed by a space for each iteration.
#/

sum = 0

repeat 10 as i do
  sum += i;

print sum

# output: 55
```

## `while`

The `while`-loop is used to loop based on a condition.

##### Loop Based on a Condition

```hayward
i = 0
while i <= 10 do
  i += 1
  println(i);
```

##### Infinite Loops

To loop indefinitely, the loop condition expression must evaluate to `true`.

```hayward
import "time" # for `delay()`
while true do
  print("\rPress Ctrl+C to exit!")
  time::delay(500); # sleep for 500 seconds
```

##### Exit a Loop

Exiting a loop with the `break` keyword.

```hayward
i = 0
while true do
  i += 1

  if i == 2
    break;
;
```

##### Loop Continuation
Skipping iterations with the `next` keyword.
```hayward
i = 0
while true do
  i += 1

  if i % 2 == 0
    next;

  println(i)

  if i >= 10
    break;
;
```

## `for`

In Hayward, `for`-loops are used to iterate collections.

##### Iterating a Hashmap

Loop on the `.keys()` builtin to iterate the keys of a hashmap.

```hayward
# Iterate the keys in the hashmap.
for key in myHashmap.keys() do
  println("${key}: ${myHashmap[key]}");

# Iterate the keys in the hashmap, with an index.
for key, index in myHashmap.keys() do
  println("Key ${index}: ${key}");
```

Loop on the `.values()` builtin to iterate the values of a hashmap.

```hayward
# Iterate the values in the hashmap.
for value in myHashmap.values() do
  println(value);

# Iterate the values in the hashmap, with an index.
for value, index in myHashmap.values() do
  println("Value ${index}: ${value}");
```

##### Iterating a List

Use the `for` keyword to iterate a list.

```hayward
fruits = ["kiwi", "mango", "lime"]

# Iterate the values in the list.
for fruit in fruits do
  println(fruit)
end

# Iterate the values in the list, with an index.
for fruit, index in fruits do
  println("Fruit ${index}: ${item}")
end
```

##### Iterating a range

Use the `for` keyword to iterate a range.

```hayward
for i in [1 to 10] do
  println("${i}")
end
```

To learn more about collections, see [Hashmaps](hashmaps.md), [Lists](lists.md), and [Ranges](ranges.md).