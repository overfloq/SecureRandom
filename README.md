# SecureRandom
A set of classes providing a Secure Random functionality for crypto pursposes for C# (.NET 7)

# How to use

Add an using decleration and create a new instance of SecureRandom.
```cs
using Crypto.RNG;

var secureRandom =
    // You can create a new instance,
    new SecureRandom();
    // or you can use a shared instance.
    SecureRandom.Shared;
```

Generate a random number
```cs
// Generate a cryptographically secure random integer within a specific range [5..10), which means value is higher or equal to 5 and less than 10.
int number = secureRandom.Next(5, 10);
Console.WriteLine($"The number is {number}");
```
