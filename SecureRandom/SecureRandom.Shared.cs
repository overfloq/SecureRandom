namespace Crypto.RNG
{
    public partial class SecureRandom
    {
        static readonly SecureRandom _shared;

        /// <summary>
        /// Shared and threadsafe instance of SecureRandom
        /// </summary>
        public static new SecureRandom Shared => _shared;

        static SecureRandom()
        {
            _shared = new SecureRandom();
        }
    }
}