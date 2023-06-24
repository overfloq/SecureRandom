using System.Runtime.CompilerServices;

namespace Crypto.RNG
{
    public partial class SecureRandom
    {
        /// <summary>
        /// Same functionality as <see cref="System.Security.Cryptography.CryptographicOperations.FixedTimeEquals(ReadOnlySpan{byte}, ReadOnlySpan{byte})"/>. Compares two sequences with same length using a fixed-time comparasion algorithm.
        /// </summary>
        /// <param name="left">Fírst sequence to be compared</param>
        /// <param name="right">Second sequence to be compared</param>
        /// <returns><c>true</c> if two sequences are equal, otherwise <c>false.</c></returns>
        /// <exception cref="ArgumentNullException"/>
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static bool Compare(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));

            if (right == null)
                throw new ArgumentNullException(nameof(right));

            if (left.Length != right.Length)
                return false;

            int length = left.Length;
            int accum = 0;

            for (int i = 0; i < length; i++)
            {
                accum |= left[i] - right[i];
            }

            return accum == 0;
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void ZeroMemory<T>(Span<T> buffer) where T : struct
        {
            buffer.Clear();
        }
    }
}