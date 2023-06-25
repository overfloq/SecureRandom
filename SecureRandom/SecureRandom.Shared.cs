namespace CryptoRandom;

public partial class SecureRandom
{
    static readonly SecureRandom _shared;

    /// <summary>
    /// Shared instance of SecureRandom, which can be used from any place of the code
    /// </summary>
    public static new SecureRandom Shared => _shared;

    static SecureRandom()
    {
        _shared = new SecureRandom(false);
    }
}