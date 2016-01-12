# Records #

Records are a kind of anonymous data type, with named fields.

```
var r = record(id : Int, name : String, balance : Float) {
  42, "Christopher Diggins", 10.0
} 
```

There is currently no way to express the type of a record, so it can only be assigned to an `Any` [variant](Variants.md) (i.e. undeclared types).

