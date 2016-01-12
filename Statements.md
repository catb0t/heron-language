# Introduction #

In addition to the [control flow statements](ControlFlow.md) Heron supports the following statement types:

  * Variable declaration
  * Expression statement
  * Code block

## Variable Declaration ##

Heron variable declaration have the following form:

```
var myVarName : SomeType = new SomeType();
```

The type declaration (e.g. `: SomeType`) and initializer (e.g. `= new SomeType()`) are both optional. So the following are examples of valid variable declarations.

```
var x;
var x : Int;
var x : Int = 42;
var x = 42;
```

When an initializer is omitted the variable is initialized with a default value which depends on the type of the variable, or is `null` in the case of an undeclared type or a reference type.

## Expression Statement ##

Any expression terminated by a semicolon is an expression statement.

For example

```
12;
x = 3;
f(a, 1);
```

## Code Blocks ##

Multiple statements can be grouped together using curly braces. This is called a code block.

For example:

```
var x = 1;
{
  var y = 2;
  y = x + 3;
}
// y is no longer valid
```