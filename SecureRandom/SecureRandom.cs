using System.Security.Cryptography;

namespace Crypto.RNG
{
    /// <summary>
    /// Threadsafe implementation of <see cref="SecureRandom"/> for generating cryptographically strong random numbers.
    /// </summary>
    public partial class SecureRandom : Random, IDisposable
    {
        private RandomNumberGenerator? _source;

        /// <summary>
        /// Creates a new instance of <see cref="SecureRandom"/> with already existing source.
        /// </summary>
        public SecureRandom(RandomNumberGenerator source)
        {
            _source = source;
        }

        /// <summary>
        /// Creates a new instance of <see cref="SecureRandom"/> with a new source.
        /// </summary>
        public SecureRandom() : this(RandomNumberGenerator.Create()) { }
        private SecureRandom(bool isShared) : this(RandomNumberGenerator.Create())
        {
            _isShared = isShared;
        }

        private readonly bool _isShared = true;

        /// <summary>
        /// Disposes current <see cref="SecureRandom"/> instance.
        /// </summary>
        public void Dispose()
        {
            if (!_isShared)
                throw new InvalidOperationException("Shared instance of SecureRandom cannot be disposed.");

            GC.SuppressFinalize(this);
            _source?.Dispose();
            _source = null;
        }

        public override bool Equals(object? obj)
        {
            return this == obj;
        }

        public override int GetHashCode()
        {
            return _source?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return GetType().ToString();
        }
    }
}