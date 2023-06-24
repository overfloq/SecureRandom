# SecureRandom
A set of classes providing a **Secure Random** functionality for crypto pursposes for **C# (.NET 7)**

# How to use
Add an **using decleration** and create a new instance of SecureRandom.
```cs
using Crypto.RNG;

// Create a new instance of SecureRandom
var secureRandom = new SecureRandom();
// If you want, you can use SecureRandom.Shared instead of creating a new instance
```

Generate a random number
```cs
// Generate a cryptographically secure random integer within a specific range [5..10), which means value is higher or equal to 5 and less than 10
int number = secureRandom.Next(5, 10);
Console.WriteLine($"The number is {number}");
```

Using extension methods
```cs
// Fill a Span (or array) with a random bytes
Span<byte> key = stackalloc byte[32];
key.FillRandom();
Console.WriteLine($"A randomly generated key is {Convert.ToBase64String(key)}");

// Shuffle Span or Array
char[] stringChars = "Hello, World!".ToCharArray();
stringChars.Shuffle();
Console.WriteLine($"Shuffled output is {string.Concat(stringChars)}");

// Securely compare two Arrays or Spans
var originalKey = new byte[32];
var enteredKey = ...;

// The code is originally taken from System.Security.Cryptography.CryptographicOperations.FixedTimeEquals
if (originalkey.SecureEquals(enteredKey)) {
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Keys are the same!");
} else {
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Sorry, but the keys aren't equal.");
}
```

## Don't forget to dispose the SecureRandom
If you weren't using a shared instance, keep in mind disposing your SecureRandom instance. You can use such things, like `using (SecureRandom secureRandom = new()) { ... }`. **Do not dispose shared instance as it will no longer be available.**
