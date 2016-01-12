# Modules #

A Heron module is a container of type definitions that can contains variables and methods that are accessible from the scope of class instances in the module. A file can contain only one module. Every program consists of at least one module.

Unlike modules in other languages, module are not singletons and their member data is not include the global scope.

Modules can inherit from another module, and can import other modules.

A module has the following layout:

```
module MyModule
{
  inherits
  { }
  imports
  { } 
  fields 
  { }
  methods
  { }
}

// Type declarations
// All types that follow a module instance in a file, belong
// to that module

class SomeClass 
{ }
```

## Accessing Module Data ##

Every class instance is associated with a single module instance. A class instance can access its module's fields, without requiring qualification as so:

```
module MyModule {
  fields {
    x : int = 42; 
  }
}

class SomeClass { 
  functions {
    GetX() : Int { 
      return x;
    }
  }
}
```

This means that every class instance has a hidden module instance pointer.

## Module Aliases ##

A module alias is a name that is assigned to a particular module instance. Similar to a module or class field. When a module is imported it can be assigned an alias as follows:

```
import {
  HelloWorld as hw;
}
```

If no alias is provided, then the module name is used as the alias.

## Module Imports and Module Instantiation ##

When a module imports another module it must instantiate the other module before it can use it. This is done using the `new` operator. Module constructors are any functions with the reserved name `Constructor` and can accept arguments.

Modules can be instantiated at the import location:

```
import {
  HelloWorld as hw = new HelloWorld();
}
```

In which case the order of initialization is that same as the order in which the imports declarations occur.

Or it can be done in the constructor.

```
methods {
  Constructor() {
    hw = new HelloWorld();
  }
}
```

## Class Instantiation and Modules ##

When a class is instantiated from another module a special form of the `new` operator is required:

```
Constructor() {
  oc = new OtherClass() from OtherModule; 
}
```

## Static Data and Module Variables ##

In Heron there is no static or global data, instead module variables serve this purpose. Each class instance is associated with a particular module instance. All class instances associated with a particular module instance have access to the same data.

Module variables have similar semantics to global functions and variables in other languages, but the difference is that a module can be instantiated multiple times.

This is an idea which was inspired by [this post by Gilad Bracha](http://gbracha.blogspot.com/2008/02/cutting-out-static.html) the author of the [Newspeak](http://newspeaklanguage.org/) language.

The other inspiration for the Heron module system is the Python module system, which has strengths and weaknesses. [See here for an interesting look at Python modules](http://washort.twistedmatrix.com/2011/01/modules-in-python-good-bad-and-ugly.html).

## How Classes Differ from Modules ##

Modules bear a resemblance to classes: they can be instantiated, they have fields and member functions. However there are some differences:

  * Only modules can contain classes, interfaces, and enums
  * Every class instance is associated with a module instance
  * A class instance can access the fields and methods of the module with which it is associated without needing to qualify them.
  * Modules allows names from other module instances to be imported

## How Modules are Loaded ##

Modules are loaded using a module searching mechanism determined by the implementation. In the current implementation, the Config.xml file accepts an "inputpath" element which contains a sequence of "path" elements to be search in order. In addition the Heron implementation will first search in the exe folder, and a sub-folder of the the exe folder named "lib". For more information see the implementation file HeronConfig.cs.

## FAQ ##

> Q: Since a module can be instantiated multiple times, is there an easy way to say   "instantiate module HelloWorld, except if it is already instantiated in another module in which case reuse the existing instance"?

> A: No. That would create a coupling between modules, that could lead to unpredictable results. The module system is designed to eliminate implicit coupling and improve software stability.

> Q: Why does "Every program consists of at least one module"?

> A: A program by definition is a single module, and all of the modules inherited and imported by that module directly or indirectly. There is no "program" entity.










