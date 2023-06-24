using System.Security.Cryptography;

namespace Crypto.RNG
{
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

        /// <summary>
        /// Disposes current <see cref="SecureRandom"/> instance.
        /// </summary>
        public void Dispose()
        {
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