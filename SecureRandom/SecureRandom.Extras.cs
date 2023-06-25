using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace CryptoRandom;

public partial class SecureRandom
{
    /// <inheritdoc cref="CryptographicOperations.FixedTimeEquals(ReadOnlySpan{byte}, ReadOnlySpan{byte})"/>
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static bool FixedTimeEquals(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
        => CryptographicOperations.FixedTimeEquals(left, right);

    /// <inheritdoc cref="CryptographicOperations.ZeroMemory(Span{byte})"/>
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static void ZeroMemory(Span<byte> buffer)
        => CryptographicOperations.ZeroMemory(buffer);
}