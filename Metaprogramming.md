# Metaprogramming #

Heron has a somewhat novel metaprogramming system that allows full introspection of the AST at compile-time, and compile-time code generation. At compile-time arbitrary code from a program can be executed and the abstract syntax tree (AST) can be transformed.

At compile-time, after the AST is first constructed by the compiler, it look for a function named `Meta()` in the target module. Just like at run-time it looks for a function named `Main()` in the target module. The `Meta()` function receives as an argument an object of type `ProgramDefn` which is a representation of the program's code model (i.e. the abstract syntax tree). The `Meta()` function has no limitations and can call any part of a Heron program.

This rather powerful metaprogramming system is offered as a replacement of run-time reflection capabilities found in other languages. The fact that Heron does not allow run-time reflection, makes it easier to optimize (rewrite) code.

## Applications ##

Metaprogramming systems have a wide range of uses. In Heron the metaprogramming system is intended to enable certain features which are built-in to other languages to libraries. This way programmers can extend the language by writing new compile-time transformations. Some examples of uses of the Heron metaprogramming system are:

  * Aspect oriented programming
  * Design by contract
  * Building unit test frameworks
  * Debugging (e.g. settings breakpoints and trace statements programmatically)
  * Translation to other languages
