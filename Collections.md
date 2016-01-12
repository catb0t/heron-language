# Collection Types #

## Sequence ##

The fundamental collection data type of Heron is the sequence (`Seq`). All other collections derive from it. It supports two operations, `ToList() : List` and `GetIterator() : Iterator`. You cannot instantiate a `Seq` type directly. It is commonly used to pass it an a argument, or return it from a function, that can operate on both iterators and lists. A sequence is similar to the `IEnumerable` interface in C#.

## List ##

The list (`List`) derives from `Seq`. This is the type that a Heron programmer will most commonly declare. A list supports adding (`Add()`) and removing (`Remove()`) items from the end (like a stack), and can be indexed (like an array). It also provides a ('Count()') function.

## Iterator ##

An iterator, also known as an enumerator in some languages, derives from `Seq` and is used to iterate over a sequence of items. Every sequence provides access to an iterator. An iterator supports `MoveNext() : Boolean` and `GetValue() : Any` methods.

## Range ##

A range is a  iterator over a contiguous sequence of integers and is created by using the range operator (`..`). Range operators are inclusive on both ends. However, this may change in later versions of Heron to make the upper end execlusive.

```
  var oneToTen : Seq = 
     1..10;
```

## Tuples ##

A tuple is a literal list of elements of type 'Any'.

```
  var m = [1, 'a', "Hello", 3.13];
```