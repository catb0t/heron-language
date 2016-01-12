# Hello World #

The obligatory _Hello world_ program, which prints the string "Hello World" to the console.

```
module HelloWorld 
{
  imports
  {
    console = new Heron.Windows.Console();
  }
  methods 
  {
    Main() 
    {
      WriteLine("Hello World");
    }
  }
}
```

# Fibonacci #

This program prints out the first 10 numbers in the Fibonacci sequence. It demonstrates functions signatures and the if statement.

```
module Fibonacci
{
    imports
    {
        Console = new Heron.Windows.Console();
    }
    fields
    {
        max = 10;
    }
    methods
    {
        Fib(n : Int) : Int
        {
            if (n <= 0) 
                return 0;
            else if (n == 1)
                return 1;
            else
                return Fib(n - 1) + Fib(n - 2);
        }

        Main() 
        {
            foreach (i in 0..max)
                WriteLine(Fib(i).ToString());
        }
    }
}
```

# Sieve of Eratosthenes #

This program prints out the prime numbers up to 100. It demonstrates ranges and the select operator.

```
module Sieve
{
  imports
  {
    console = new Heron.Windows.Console();
  }
  fields 
  {
    max = 10;
  }
  methods 
  {
    Main() 
    {
      var primes = 0..(max * max);
	foreach (i in 2..max)
	  primes = select (j from primes) 
            j % i != 0;		
	foreach (i in primes)
	  Console.WriteLine(i);
    }
  }
}
```