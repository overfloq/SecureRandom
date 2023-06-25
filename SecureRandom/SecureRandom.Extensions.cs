namespace Crypto.RNG
{
    public static class SecureRandomExtensions
    {
        /// <summary>
        /// Fills the sequence with a random numbers.
        /// </summary>
        public static void FillRandom(this byte[] buffer)
            => FillRandom(buffer.AsSpan());

        /// <summary>
        /// Fills the sequence with a random numbers.
        /// </summary>
        public static void FillRandom(this Span<byte> buffer)
            => SecureRandom.Shared.NextBytes(buffer);

        /// <returns>Random value from the sequence.</returns>
        public static T GetRandom<T>(this ICollection<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            return collection switch
            {
                List<T> => SecureRandom.Shared.NextElement(collection.ToList()),
                Array => SecureRandom.Shared.NextElement(collection.ToArray()),
                _ => SecureRandom.Shared.NextElement(collection)
            };
        }

        /// <returns>Random value from the sequence.</returns>
        public static T GetRandom<T>(this ReadOnlySpan<T> span)
            => SecureRandom.Shared.NextElement(span);

        /// <inheritdoc cref="SecureRandom.Shuffle{T}(Span{T})"/>
        public static void Shuffle<T>(this T[] array)
            => SecureRandom.Shared.Shuffle(array);

        /// <inheritdoc cref="SecureRandom.Shuffle{T}(Span{T})"/>
        public static void Shuffle<T>(this Span<T> span)
            => SecureRandom.Shared.Shuffle(span);

        /// <inheritdoc cref="SecureRandom.Compare(ReadOnlySpan{byte}, ReadOnlySpan{byte})"/>
        public static bool SecureEquals(this byte[] first, byte[] second)
            => SecureRandom.Compare(first.AsSpan(), second.AsSpan());

        /// <inheritdoc cref="SecureRandom.Compare(ReadOnlySpan{byte}, ReadOnlySpan{byte})"/>
        public static bool SecureEquals(this ReadOnlySpan<byte> first, ReadOnlySpan<byte> second)
            => SecureRandom.Compare(first, second);
    }
}