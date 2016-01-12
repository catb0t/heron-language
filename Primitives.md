# Primitive Types #

Primitive types are the types that are guaranteed to be available for any compliant implementation of Heron.

## Value Types ##

Heron has the following primitive value types:

  * Int - integer value
  * Float - floating point value
  * Char - character value
  * Bool - boolean value
  * Any - a dynamically typed variant type

The size of the primitive value types is implementation defined.

A value-type is distinguished from a reference type in that assignment makes a copy of the value, rather than a shared reference.

## Reference Types ##

A reference type is shared when assigned, and can be assigned to null. Heron has the following primitive reference types:

  * String - an immutable list of values
  * Type - a type of a type

### Collection Types ###

The following types are related to [collections](Collections.md).

  * Seq - a sequence, the base type of all collections
  * List - a mutable collection of value derived from Seq
  * Range - an integer generator derived from Iterator
  * Iterator - an enumerator derived from Seq

### Special Types ###

The following are special types, which can not be instantiated.

  * Void - The return-type of a function which returns nothing
  * Null - A value representing an unassigned reference.
  * Undefined - Not currently used, and may be removed

# Implementation Details #

The types of primitives are declared in the file `HeronPrimitiveTypes.cs`, and the implementations of several of the primitive values are defined in the file `HeronPrimitiveValue.cs`.