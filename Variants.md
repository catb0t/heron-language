# Any #

Heron has a variant type called `Any` which can be assigned any value. The `Any` type holds type information about the original value that can be retrieved dynamically. There are two operators that work with Any: `is` and `as`.

The `Any` type is useful in situations where [downcasting](UpcastingAndDowncasting.md) semantics is desired.

# Implementation Details #

The `Any` type is implemented in the file HeronAny.cs.