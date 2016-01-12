# The Heron Editor Mini-IDE #

Heron comes with a simple integrated development environment (IDE). This IDE provides basic text editing capabilities, such as found in notepad, with the following enhancements for working with Heron files:

  * Syntax coloring
  * On-the-fly parse error reporting
  * Multi-level undo
  * Single key-press to run a file
  * The editor can be programmatically extended via macros (scripts) written in Heron.

## Source Code ##

The HeronEdit IDE was written in C# and the source code is available with any download. Like the rest of Heron the source code is available for use under the MIT Licence 1.0.

## Screen Shot ##

![http://heron-language.com/heron-edit-april-20.png](http://heron-language.com/heron-edit-april-20.png)

## Extending HeronEdit ##

A user can run a custom macro by pressing Ctrl+M. This will open a dialog box that allows them to enter some text. This text is then passed to a Heron program called `Macros.heron` and is executed by the IDE.

The file [Macros.heron](http://code.google.com/p/heron-language/source/browse/trunk/HeronEngine/macros/Macros.heron) demonstrates a very simple script that causes the currently selected text block to become commented when the user enters `comment` in the macro dialog box.