# Upcasting and Downcasting #

An _upcast_ is a cast from a derived type to a base type. A _downcast_ is a cast from a base type to a derived type. In Heron upcasts are legal, but downcasts are not.

If a downcast is absolutely required, and an interface is not appropriate, a programmer can instead use the [`Any` variant](Variants.md).

## Rationale ##

Preventing downcasts is intended to decrease the chance of certain defects. When a module or component uses downcasts it can become reliant on hard to identify requirements (e.g. it requests an object of type X, but really wants an object of type Y). In these cases it is preferable to use interfaces.

Preventing downcasts has the advantage of making it easier to ensure that the [Liskov substitution principle](http://en.wikipedia.org/wiki/Liskov_substitution_principle) is respected.