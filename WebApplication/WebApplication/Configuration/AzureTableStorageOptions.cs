namespace WebApplication.Configuration
{
    /// <summary>
    /// Represents the setting for azure table repository.
    /// </summary>
    public sealed class AzureTableStorageOptions
    {
        /// <summary>
        /// The azure table account name.
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// The azure table account key.
        /// </summary>
        public string AccountKey { get; set; }

        /// <summary>
        /// The table name.
        /// </summary>
        public string TableName { get; set; }
    }
}