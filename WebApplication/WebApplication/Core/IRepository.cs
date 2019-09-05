namespace WebApplication.Core
{
    using System;

    /// <summary>
    /// An interface that contains method to store and retrieve the url.
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Adds the shorten & long url.
        /// </summary>
        /// <param name="shortenUrl">The unique shorten url.</param>
        /// <param name="url">The long url.</param>
        /// <returns>True if successful.</returns>
        bool Add(string shortenUrl, Uri url);

        /// <summary>
        /// Get the long url.
        /// </summary>
        /// <param name="shortenUrl">The shorten url.</param>
        /// <returns>The long url. Null if the url doesn't exists.</returns>
        Uri Get(string shortenUrl);
    }
}