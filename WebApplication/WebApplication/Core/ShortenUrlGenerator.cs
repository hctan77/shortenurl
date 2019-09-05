namespace WebApplication.Core
{
    using EnsureThat;
    using System;
    using System.Security.Cryptography;

    /// <summary>
    /// A class that generates a unique shorten url.
    /// </summary>
    internal sealed class ShortenUrlGenerator : IShortenUrlGenerator
    {
        /// <inheritdoc/>
        public string GetRandomUrl(Uri longUrl)
        {
            EnsureArg.IsNotNull(longUrl, nameof(longUrl));

            return GenerateShortUniqueKey($"{longUrl}{Guid.NewGuid()}");
        }

        private string GenerateShortUniqueKey(string input)
        {
            var hash = CalculateMD5Hash(input);

            var base64Hash = Convert.ToBase64String(hash);

            // replace / and +
            return base64Hash.Replace('/', '_').Replace('+', '-').Substring(0, 6);
        }

        private byte[] CalculateMD5Hash(string input)
        {
            // calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            return md5.ComputeHash(inputBytes);
        }
    }
}