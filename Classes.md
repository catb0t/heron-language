# Classes #

Heron class have some significant differences from other object-oriented programming languages.

  * There are no virtual functions
  * All fields and methods are public by default
  * There are no static functions
  * Constructors are always named `Constructor()` instead of the class name
  * There are no visibility specifiers

Heron classes are broken into the following sections.

  1. inherits - the inherited classes
  1. implements - the implemented interfaces
  1. fields - member fields
  1. methods - member functions

Each section is optional, but the order of sections is mandatory.

The following is an example of a simple class:

```
class Main
{
  fields
  {
    msg : String;
  }
  methods
  {
    Constructor()
    {
      msg = "Hello World";
      PrintMessage();
    }
    PrintMessage()
    {
      Console.WriteLine(mscg);
    }
  }
}
```

# Implementation Details #

The class implementation is in the class `ClassInstance` can be found in the file HeronValue.cs.