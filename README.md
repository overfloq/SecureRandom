# SecureRandom
A set of classes providing a **Secure Random** functionality for crypto pursposes for **C# (.NET 7)**

# How to use
Add an **using decleration** and create a new instance of SecureRandom. This should be avoided, when creating a lot of instances - then, you should use `SecureRandom.Shared` static property, which contains a one **shared SecureRandom instance**.
```cs
using Crypto.RNG;

// Create a new instance of SecureRandom, or use SecureRandom.Shared instead
var secureRandom = new SecureRandom();
```

### Generate a random number
```cs
// Generate a cryptographically secure random integer within a specific range [5..10), which means value is higher or equal to 5 and less than 10
int number = secureRandom.Next(5, 10);
Console.WriteLine($"The number is {number}");
```

### Use of extension methods
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

// This code is using System.Security.Cryptography.CryptographicOperations.FixedTimeEquals
if (originalKey.SecureEquals(enteredKey)) {
    // The arrays are the same length and contains exactly the same values!
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Keys are the same!");
} else {
    // The arrays are different by it's length or values.
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Sorry, but the keys aren't equal.");
}
```

## Don't forget to dispose the SecureRandom
If you weren't using a shared instance, keep in mind disposing your SecureRandom instance. You can use such things, like `using (SecureRandom secureRandom = new()) { ... }`. **Don't dispose a shared instance!**
