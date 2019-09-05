namespace WebApplication.Core.Model
{
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// A class representing the azure table link model.
    /// </summary>
    internal sealed class LinkEntity : TableEntity
    {
        public LinkEntity()
        {
        }

        public LinkEntity(string shortenUri, string url)
            : base(shortenUri.Substring(0,1), shortenUri)
        {
            Url = url;
        }

        public string Url { get; set; }
    }
}