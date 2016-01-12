# The Heron Interpreter #

This project is an open-source interpreter for the Heron language written in C#. Heron is a general purpose object-oriented programming language. Heron is similar to Java, with support for higher-order functions and dynamic typing (i.e. optional type annotations). Syntactically Heron bears a very close resemblance to ActionScript 3.0, but is not a prototype-based language.  One of the interesting features of Heron, is a metaprogramming system which allows the abstract syntax tree to be modified after the first compilation pass.

The interpreter was designed for the source to be as simple and easy as possible to for ease of maintainenance and speed of development, at the cost of efficiency. This means that it should be easy to modify for other language projects.

## Features of Heron ##

While Heron is not yet complete, the following are some of the salient features which set it apart from similar languages like Java.

### Dynamic Typing and the Heron Type System ###

Heron supports a universal variant type called "Any" which is similar to the universal subtype "Object" used in many languages. When a type is omitted the "Any" type is assumed.
Technically "Any" is not a universal subtype. It supports coercion operators to and from other types, but does not participate in the type hierarchy in the same way that other types do.

To illustrate this consider a class "D" derived from a class "B". An instance "d" of type "D", is in fact also an instance of a class "B", but is not an instance of "Any".

### Interfaces but No Virtual Functions ###

Heron does not support virtual functions. Similar functionality can be achieved by explicitly using interface classes in the base class, and having derived classes register with base classes from the constructor. This is intended to eliminate a common location of bugs in object-oriented programming, and to improve static and dynamic analysis.

### Behavioral Subtyping ###

In Heron inheritance implies behaviorial subtyping. Any instance of a class "D" that derives from another class "B" can be used where "B" would be accepted without introducing new unforseen problems. In other words Heron supports behaviorial subtyping, because classes maintain the [Liskov Substitution Principle](http://en.wikipedia.org/wiki/Liskov_substitution_principle) (LSP).

### Heron Metaprogramming ###

Heron avoids many of the costs (memory overhead, processing speed, etc.) associated with supporting run-time reflection by providing a powerful metaprogramming system at compile-time instead of using a run-time reflection system.

The Heron metaprogramming system allows full execution of a program at compile-time to modify its own abstract syntax tree (AST). This is done by providing a separate entry point at compile-time (called the "premain") distinct from the run-time entry point ("main"). The "premain" can call any function or instantiate any class in the program, and is provided with a reference to the AST which it can examine or modify.

This means that a program can be its own pretty printer, or translator, or static analyzer, or other things. Logically this would be done from within a common library.