namespace WebApplication.Core
{
    using System;

    public sealed class AzureTableRepository : IRepository
    {
        /// <inheritdoc />
        public bool Add(string shortenUrl, Uri url)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Uri Get(string shortenUrl)
        {
            throw new NotImplementedException();
        }
    }
}