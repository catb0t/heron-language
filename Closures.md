# Closures #

Heron supports closures. A function can contain variables that are bound to the lexical environment where the function was defined.

For example:

```
  var x = 12;
  var f = function() : Int { return x + 1; }
  Console.WriteLine(f()); // outputs 13
```