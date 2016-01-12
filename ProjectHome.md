# Heron #

Heron is a new general purpose programming language that aims to make large scale programming easier, faster, and safer, while also being appropriate for small scale programming tasks.

Heron has a syntax which resembles Java, C#, ECMAScript, and Scala. Heron is primarily an object-oriented language, but borrows many ideas from functional languages, and even some from array-oriented languages.

## Distinguishing Characteristics ##

The following are some of the more notable features of Heron

  * First class modules
  * Built-in concurrent list operations
  * Variant types
  * Signature-based polymorphism (i.e. duck typing)
  * Optional type signatures
  * Advanced compile-time programming support
  * Interfaces

Heron is notable in its explicit non-support for certain language features. Several common language features were omitted in order to keep the language simple, safe, and easy to optimize. This was possible because the other features of the language provide alternative mechanisms to achieve similar purposes.

  * No predefined visibility specifiers
  * No run-time reflection
  * No assignment to null by default
  * No universal base type (e.g. 'Object')
  * No virtual functions
  * No downcasts (i.e. casting from a Base type to a Derived type)
  * No virtual functions
  * No static data
  * No global data

## The Heron Interpreter ##

This project provides the source code and executable for an interpreter for Heron written in C# using .NET 4.0. As a language, Heron is in no way tied to the common language run-time or the Windows Platform. Heron can be ported to any platform with relative ease.

Documentation and learning resources are hosted on the Wiki. To start see the Wiki TableOfContents page.

## The Heron Editor Mini-IDE ##

Heron comes with a simple integrated development environment (IDE). This IDE provides basic text editing capabilities, such as found in notepad, with the following enhancements for working with Heron files:

  * Syntax coloring
  * On-the-fly parse error reporting
  * Multi-level undo
  * Single key-press to run a file
  * The editor can be programmatically extended via macros (scripts) written in Heron.

For more information see the [Heron edit](HeronEdit.md) page.