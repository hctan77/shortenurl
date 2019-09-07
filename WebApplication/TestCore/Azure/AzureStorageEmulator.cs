namespace TestCore.Azure
{
    using System;

    using Microsoft.WindowsAzure.Storage;

    /// <summary>
    /// A class for setup of Azure Storage Emulator.
    /// </summary>
    public sealed class AzureStorageEmulator : IDisposable
    {
        private const string EmulatorConnectionString = "UseDevelopmentStorage=true";

        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureStorageEmulator"/> class.
        /// </summary>
        public AzureStorageEmulator()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(EmulatorConnectionString);
            this.TableClient = new GuardTableClient(storageAccount.CreateCloudTableClient());

            if (AzureStorageEmulatorManager.IsProcessRunning())
            {
                StopStorageEmulator();
            }

            StartStorageEmulator();
        }

        public GuardTableClient TableClient { get; }

        public void Dispose()
        {
            if (!this.disposed)
            {
                this.TableClient.DeleteCreatedTables();

                StopStorageEmulator();
                this.disposed = true;
            }
        }

        private static void StopStorageEmulator()
        {
            AzureStorageEmulatorManager.StopStorageEmulator();
        }

        private static void StartStorageEmulator()
        {
            AzureStorageEmulatorManager.StartStorageEmulator();
        }
    }
}