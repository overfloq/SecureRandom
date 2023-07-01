### This repository is no longer maintained by me, however, you still can use the code. .NET 7 provides a RandomNumberGenerator.GetInt32 for generating an integer within a specified range. It is recommended to use .NET's static cryptography method rather than this SecureRandom, which is even slower. The only advantages it provides are extension methods and other methods, like NextEnum or NextDouble.

# SecureRandom
A set of classes providing a **Secure Random** functionality for crypto pursposes for **C# (.NET 7)**

# How to use
Add an **using decleration** and create a new instance of SecureRandom. This should be avoided, when creating a lot of instances - then, you should use `SecureRandom.Shared` static property, which contains a one **shared SecureRandom instance**.
```cs
using CryptoRandom;

// Instead use SecureRandom.Shared to get a shared instance
using var secureRandom = new SecureRandom();
Console.WriteLine($"My random number is {secureRandom.Next(0, 10)}"); // Generate a random integer in range of [0..10)
```

## Don't forget to dispose the SecureRandom
If you were not using a shared instance, keep in mind disposing your SecureRandom instance. You can use such things, like `using (SecureRandom secureRandom = new()) { ... }`. **Don't dispose a shared instance!**

*More descriptive example can be found in `Program.cs`*
