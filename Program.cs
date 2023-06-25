﻿// The SecureRandom namespace
using Crypto.RNG;

// Allocate a new span and fill it with a random bytes
Span<byte> key = stackalloc byte[16];
key.FillRandom();

Console.WriteLine($"A randomly generated byte sequence (base64) is {Convert.ToBase64String(key)}");

// Shuffle an array
char[] stringChars = "Random".ToCharArray();
stringChars.Shuffle();
Console.WriteLine($"Shuffled char sequence is: \"{string.Concat(stringChars)}\"");

// Allocate two arrays, that'll be compared soon
var originalKey = new byte[32];
var enteredKey = new byte[32];

// Make one byte different
enteredKey[3] = 0x03;

// Write a result of a secure fixed-time array comparasion
Console.WriteLine(originalKey.SecureEquals(enteredKey) ?
    "SUCCESS: Keys are matching!" : "ERROR: Keys are different.");

/*   The extension methods (like Array.FillRandom) are using a shared instance of SecureRandom.
 *   Always dispose the instance, you've created. Shared instance cannot be disposed.
 *   
 *   If we need a local variable, that has an instance of SecureRandom, we can either create
 *   an instance, so 'var secureRandom = new SecureRandom()', or, instead of using new, we can just copy reference
 *   of the shared instance of SecureRandom to make usage easier, so 'var secureRandom = SecureRandom.Shared'.
 */

var secureRandom = new SecureRandom();

var probability = secureRandom.Probability(0.5);
Console.WriteLine($"We have only 50% of having a chance to be following value True: '{probability}'");

/*
 *   Dispose the created instance, NOT shared instance. Disposing will destroy the source even, if we passed it
 *   as an argument, like 'new SecureRandom(RandomNumberGenerator source)'.
 */
secureRandom.Dispose();

/*
 *   By destroying a source, we no longer can receive next bytes, so things, like NextBytes, Next,
 *   Probability and etc.. will no longer be working properly.
 */

byte[] newArray = new byte[32];

// We are not gonna use Shared instance (Array.FillRandom), but the already disposed instance of SecureRandom
secureRandom.NextBytes(newArray);

/*
 *   As you can see, the array will stay not changed and no exception will be thrown.
 *   If we do the same with numbers, output will be the lowest possible value.
 */

Console.WriteLine($"Output from disposed SecureRandom : {Convert.ToBase64String(newArray)}");
Console.WriteLine($"Also a number [-10..10)           : {secureRandom.Next(-10, 10)}");

/*   SecureRandom.Shared.Dispose();
 *                ^^^^^^ ^^^^^^^
 *   By running this code, an exception InvalidOperationException will be
 *   thrown, because Shared instance of SecureRandom cannot be disposed.
 */

Console.Read();