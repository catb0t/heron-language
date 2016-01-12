# Tables #

A table is a collection of [records](Records.md) each with the same layout.

```
var accounts = table(id : Int, name : String, balance : Float) {
  1, "Christopher Diggins", 10.0;
  2, "Melanie Charbonneau", 22.0;
  3, "Anna Diggins", 2400.5;
  4, "Beatrice Diggins", 99.99;
} 
```

You would access individual records of a table as so:

```
foreach (i in 1..4)
  WriteLine(accounts[i].name);
```

A table provides fast look-up based on the first column. So a two column table is effectively the same as a hash table or dictionary in other languages. The only caveat is that

```
function StringToInt(s : String) : Int {
  var numbers = table(key : String, value : Int) {
    "one",   1;
    "two",   2; 
    "three", 3; 
    "four",  4; 
    "five",  5;
  }
  return numbers[s].value;
}
```