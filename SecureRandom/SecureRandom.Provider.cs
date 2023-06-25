using System.Buffers.Binary;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace CryptoRandom;

public partial class SecureRandom
{
    /// <summary>
    /// Fills the sequence with random numbers.
    /// </summary>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="ArgumentOutOfRangeException"/>
    /// <exception cref="ArgumentException"/>
    public override void NextBytes(byte[] buffer)
        => NextBytes(buffer, 0, buffer.Length);

    /// <summary>
    /// Fills the specified sequence with random numbers, starting from the specified offset and count.
    /// </summary>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="ArgumentOutOfRangeException"/>
    /// <exception cref="ArgumentException"/>
    public void NextBytes(byte[] buffer, int offset, int count)
        => _source?.GetBytes(buffer, offset, count);

    /// <summary>
    /// Fills the sequence with random numbers.
    /// </summary>
    public override void NextBytes(Span<byte> buffer)
        => _source?.GetBytes(buffer);

    /// <summary>
    /// Fills the specified sequence with random numbers, starting from the specified offset and count.
    /// </summary>
    public void NextBytes(Span<byte> buffer, int offset, int count)
        => _source?.GetBytes(buffer.Slice(offset, count));

    private int NextInt()
    {
        Span<byte> bytes = stackalloc byte[sizeof(int)];
        NextBytes(bytes);
        return (int)BinaryPrimitives.ReadUInt32LittleEndian(bytes);
    }

    /// <summary>
    /// Random non-negative <see cref="int"/> value.
    /// </summary>
    public override int Next()
        => NextInt() & int.MaxValue;

    /// <summary>
    /// Random <see cref="int"/> value within a range <c>[0..toExclusive)</c>.
    /// </summary>
    /// <exception cref="ArgumentException"/>
    public override int Next(int toExclusive)
    {
        if (toExclusive <= 0)
            throw new ArgumentOutOfRangeException(nameof(toExclusive), "toExclusive <= 0");

        return Next(0, toExclusive);
    }

    /// <summary>
    /// Random <see cref="int"/> value within a range <c>[fromInclusive..toExclusive)</c>.
    /// </summary>
    /// <exception cref="ArgumentException"/>
    public override int Next(int fromInclusive, int toExclusive)
    {
        if (fromInclusive >= toExclusive)
            throw new ArgumentException("fromInclusive >= toExclusive");

        uint range = (uint)toExclusive - (uint)fromInclusive - 1;

        if (range == 0)
        {
            return fromInclusive;
        }

        uint mask = range;
        mask |= mask >> 1;
        mask |= mask >> 2;
        mask |= mask >> 4;
        mask |= mask >> 8;
        mask |= mask >> 16;

        uint oneUint = 0;
        Span<byte> oneUintBytes = MemoryMarshal.AsBytes(new Span<uint>(ref oneUint));
        uint result;

        do
        {
            _source?.GetBytes(oneUintBytes);
            result = mask & oneUint;
        }
        while (result > range);

        return (int)result + fromInclusive;
    }

    /// <summary>
    /// Random <see cref="double"/> value within a range <c>[0..1)</c>.
    /// </summary>
    public override double NextDouble()
    {
        Span<byte> bytes = stackalloc byte[sizeof(ulong)];
        NextBytes(bytes);

        var ul = BinaryPrimitives.ReadUInt64LittleEndian(bytes) / (1 << 11);
        double d = ul / (double)(1UL << 53);
        return d;
    }

    /// <summary>
    /// Random <see cref="float"/> value within a range <c>[0..1)</c>.
    /// </summary>
    public override float NextSingle()
    {
        Span<byte> buffer = stackalloc byte[sizeof(uint)];
        NextBytes(buffer);

        uint randomUInt = BinaryPrimitives.ReadUInt32LittleEndian(buffer);
        float randomValue = (float)(randomUInt / (uint.MaxValue + 1.0));
        return randomValue;
    }

    /// <summary>
    /// Random <see cref="long"/> value within a range <c>[<see cref="long.MinValue"/>..<see cref="long.MaxValue"/>)</c>.
    /// </summary>
    public override long NextInt64()
    {
        Span<byte> bytes = stackalloc byte[sizeof(long)];
        NextBytes(bytes);
        long randomNumber = BinaryPrimitives.ReadInt64LittleEndian(bytes);

        return randomNumber;
    }

    /// <summary>
    /// Random <see cref="long"/> value within a range <c>[0..<see cref="ulong.MaxValue"/>)</c>.
    /// </summary>
    public ulong NextUInt64()
    {
        Span<byte> bytes = stackalloc byte[sizeof(ulong)];
        NextBytes(bytes);
        ulong randomNumber = BinaryPrimitives.ReadUInt64LittleEndian(bytes);

        return randomNumber;
    }

    /// <summary>
    /// Random <see cref="long"/> value within a range <c>[0..toExclusive)</c>.
    /// </summary>
    /// <exception cref="ArgumentException"/>
    public override long NextInt64(long toExclusive)
        => NextInt64(0, toExclusive);

    /// <summary>
    /// Random <see cref="long"/> value within a range <c>[fromInclusive..toExclusive)</c>.
    /// </summary>
    /// <exception cref="ArgumentException"/>
    public override long NextInt64(long fromInclusive, long toExclusive)
    {
        if (fromInclusive == toExclusive)
            return toExclusive;
        if (fromInclusive > toExclusive)
            throw new ArgumentException(nameof(fromInclusive) + " is greater than " + nameof(toExclusive));

        long result = Next((int)(fromInclusive >> 32), (int)(toExclusive >> 32));
        result <<= 32;
        result |= (long)Next((int)fromInclusive, (int)toExclusive);
        return result;
    }

    /// <summary>
    /// Random <see cref="Enum"/> value.
    /// </summary>
    /// <exception cref="ArgumentException"/>
    public TEnum NextEnum<TEnum>() where TEnum : Enum
    {
        Array values = typeof(TEnum).GetEnumValuesAsUnderlyingType();
        return (TEnum?)values.GetValue(Next(values.Length)) ?? throw new ArgumentNullException(nameof(values));
    }

    /// <summary>
    /// Performs a Fisher-Yates array shuffling algorithm on the sequence.
    /// </summary>
    public void Shuffle<T>(T[] buffer)
        => Shuffle(buffer.AsSpan());

    /// <summary>
    /// Performs a Fisher-Yates array shuffling algorithm on the sequence.
    /// </summary>
    public void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Next(n + 1);

#pragma warning disable IDE0180
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
#pragma warning restore IDE0180
        }
    }

    /// <summary>
    /// Performs a Fisher-Yates array shuffling algorithm on the sequence.
    /// </summary>
    public void Shuffle<T>(Span<T> buffer)
    {
        int n = buffer.Length;
        while (n > 1)
        {
            n--;
            int k = Next(n + 1);

#pragma warning disable IDE0180
            T value = buffer[k];
            buffer[k] = buffer[n];
            buffer[n] = value;
#pragma warning restore IDE0180
        }
    }

    /// <summary>
    /// Returns a <see cref="bool"/>, which represents, if chance specified has passed.
    /// </summary>
    public bool Probability(float probability)
        => Probability((double)probability);

    /// <inheritdoc cref="Probability(float)"/>
    public bool Probability(double probability)
        => NextDouble() <= Math.Max(0, Math.Min(1, probability));

    /// <summary>
    /// Random <see cref="bool"/> value.
    /// </summary>
    public bool NextBoolean()
        => Next(2) == 1;

    /// <summary>
    /// Random element from the sequence.
    /// </summary>
    /// <exception cref="ArgumentException"/>
    public TSource NextElement<TSource>(TSource[] array)
        => NextElement<TSource>(array.AsSpan());

    /// <inheritdoc cref="NextElement{TSource}(TSource[])"/>
    public TSource NextElement<TSource>(ReadOnlySpan<TSource> span)
    {
        if (span.Length == 0)
            throw new ArgumentException("Span has length of value 0", nameof(span));

        return span[Next(span.Length)];
    }

    /// <inheritdoc cref="NextElement{TSource}(TSource[])"/>
    public TSource NextElement<TSource>(List<TSource> collection)
    {
        if (collection.Count == 0)
            throw new ArgumentException("List has length of value 0", nameof(collection));

        return collection[Next(collection.Count)];
    }

    /// <inheritdoc cref="NextElement{TSource}(TSource[])"/>
    public TSource NextElement<TSource>(ICollection<TSource> collection)
    {
        if (collection.Count == 0)
            throw new ArgumentException("Collection has length of value 0", nameof(collection));

        return collection.ElementAt(Next(collection.Count));
    }
}