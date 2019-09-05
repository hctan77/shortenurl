namespace WebApplication.Core
{
    using System;

    /// <summary>
    /// An interface that contains method to generate the shorten url.
    /// </summary>
    public interface IShortenUrlGenerator
    {
        /// <summary>
        /// Generate a shorten random url.
        /// </summary>
        /// <param name="longUrl">The long url.</param>
        /// <returns>The shorten url.</returns>
        string GetRandomUrl(Uri longUrl);
    }
}