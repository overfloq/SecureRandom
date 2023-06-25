namespace CryptoRandom.Extensions;

public static class SecureRandomExtensions
{
    /// <summary>
    /// Fills the sequence with a random numbers.
    /// </summary>
    public static void FillRandom(this Span<byte> buffer)
        => SecureRandom.Shared.NextBytes(buffer);

    /// <returns>Select a random value from the sequence.</returns>
    /// <exception cref="ArgumentException"/>
    public static T PickRandom<T>(this ICollection<T> collection)
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));

        return collection switch
        {
            List<T> list => SecureRandom.Shared.NextElement(list),
            T[] array => SecureRandom.Shared.NextElement(array),
            _ => SecureRandom.Shared.NextElement(collection)
        };
    }

    /// <returns>Select a random value from the sequence.</returns>
    public static T PickRandom<T>(this ReadOnlySpan<T> span)
        => SecureRandom.Shared.NextElement(span);

    /// <inheritdoc cref="SecureRandom.Shuffle{T}(Span{T})"/>
    public static void Shuffle<T>(this T[] array)
        => SecureRandom.Shared.Shuffle(array);

    /// <inheritdoc cref="SecureRandom.Shuffle{T}(Span{T})"/>
    public static void Shuffle<T>(this Span<T> span)
        => SecureRandom.Shared.Shuffle(span);

    /// <inheritdoc cref="SecureRandom.Shuffle{T}(Span{T})"/>
    public static void Shuffle<T>(this List<T> list)
        => SecureRandom.Shared.Shuffle(list);

    /// <inheritdoc cref="SecureRandom.Shuffle{T}(Span{T})"/>
    public static string Shuffle(this string str)
    {
        var sequence = str.ToCharArray();
        SecureRandom.Shared.Shuffle(sequence);

        return string.Concat(sequence);
    }

    /// <inheritdoc cref="SecureRandom.FixedTimeEquals(ReadOnlySpan{byte}, ReadOnlySpan{byte})"/>
    public static bool FixedTimeEquals(this ReadOnlySpan<byte> first, ReadOnlySpan<byte> second)
        => SecureRandom.FixedTimeEquals(first, second);

    /// <inheritdoc cref="SecureRandom.FixedTimeEquals(ReadOnlySpan{byte}, ReadOnlySpan{byte})"/>
    public static bool FixedTimeEquals(this Span<byte> first, Span<byte> second)
        => SecureRandom.FixedTimeEquals(first, second);

    /// <inheritdoc cref="SecureRandom.FixedTimeEquals(ReadOnlySpan{byte}, ReadOnlySpan{byte})"/>
    public static bool FixedTimeEquals(this byte[] first, byte[] second)
        => SecureRandom.FixedTimeEquals(first, second);
}