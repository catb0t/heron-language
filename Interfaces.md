# Interfaces #

Interfaces in Heron are similar to interfaces in Java and C#. They are especially important in Heron because there are no virtual functions. A class declares the interfaces it implements in an `implements` section. Interfaces can inherit from one or more other interfaces.

An example of an interface is:

```
interface IColoredPoint {
  inherits {
    IPoint;
  }
  methods {
    GetColor() : Color;
    SetColor(x : Color);
  }
}
```