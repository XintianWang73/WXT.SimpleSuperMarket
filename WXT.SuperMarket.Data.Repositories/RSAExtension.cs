namespace WXT.SuperMarket.Data.Repository
{
    using System.Security.Cryptography;

    /// <summary>
    /// Defines the <see cref="RSAExtension" />
    /// </summary>
    public static class RSAExtension
    {
        /// <summary>
        /// The EncryptRSA
        /// </summary>
        /// <param name="source">The source<see cref="byte[]"/></param>
        /// <param name="key">The key<see cref="string"/></param>
        /// <param name="fOAEP">The fOAEP<see cref="bool"/></param>
        /// <returns>The <see cref="byte[]"/></returns>

        private static readonly RSACryptoServiceProvider _rsa = new RSACryptoServiceProvider(new CspParameters
        {
            KeyContainerName = "HappyNewYear"
        });

        public static byte[] EncryptRSA(this byte[] source, bool fOAEP = false)
        {
            if (source == null)
            {
                return null;
            }
            using (_rsa)
            {
                return (_rsa.Encrypt(source, fOAEP));
            }
            
        }

        /// <summary>
        /// The DecryptRSA
        /// </summary>
        /// <param name="source">The source<see cref="byte[]"/></param>
        /// <param name="key">The key<see cref="string"/></param>
        /// <param name="fOAEP">The fOAEP<see cref="bool"/></param>
        /// <returns>The <see cref="byte[]"/></returns>
        public static byte[] DecryptRSA(this byte[] source, bool fOAEP = false)
        {
            if (source == null)
            {
                return null;
            }
            using (_rsa)
            {
                return (_rsa.Decrypt(source, fOAEP));
            }
        }
    }
}
