# Control Flow #

Heron has the following control flow constructs:

  * for
  * while
  * foreach
  * if / else
  * switch / case / default

## For Statements ##

A for statement initializes an index value, and executes a loop body repeatedly while an invariant expression is true. Each iteration of the loop it updates the index value.
A `for` statement has similar syntax to its counterpart in Java and C or C++.

```
  for (int i=0; i < 10; ++i)
    WriteLine(i.ToString();
```

## While Statement ##

A `while` statement executes a loop body repeatedly while an invariant expression is true.

```
  ch = ReadChar();
  while (ch != 'x') {
     Write(ch.ToString());
  }
```

## Foreach Statements ##

The `foreach` statement executes a statement for each item in a list consecutively.

```
  foreach (x in 0..99)
    WriteLine(99 - x + " bottles of beer on the wall");
```

Unlike the sequence operators like `map`, it is guaranteed to execute the statement for every item in the sequence and in order.

## If statement ##

The `if` statement executes a statement if a condition is true. It can be extended with an `else` clause which is executed if the the condition is false.

```
  if (IsMonday())
     WriteLine("Do you have a case of the Mondays?");
  else
    WriteLine("At least it isn't Monday.");
```

## Switch Statement ##

The switch statement compares a switch expression evaluation with a value associated with each case statement. It chooses the first case statement for which the values are equivalent.
