namespace TestCore.Azure
{
    using System.Diagnostics;
    using System.IO;

    /// <summary>
    /// A class representing the factory to create Azure Storage Emulator process.
    /// </summary>
    internal static class AzureStorageEmulatorProcessFactory
    {
        private const string EmulatorDirectoryPath = @"C:\Program Files (x86)\Microsoft SDKs\Azure\Storage Emulator";
        private const string NewEmulatorName = "AzureStorageEmulator.exe";
        private const string OldEmulatorName = "WAStorageEmulator.exe";

        public static ProcessStartInfo Create(ProcessCommand command)
        {
            string filePath = Path.Combine(EmulatorDirectoryPath, NewEmulatorName);

            if (!File.Exists(filePath))
            {
                filePath = Path.Combine(EmulatorDirectoryPath, OldEmulatorName);
            }

            return new ProcessStartInfo
            {
                FileName = filePath,
                Arguments = GetCommandArgument(command),
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
        }

        private static string GetCommandArgument(ProcessCommand command)
        {
            return command == ProcessCommand.Init ?
                @"init /server (localdb)\MSSQLLocalDb /forceCreate" :
                command.ToString().ToUpperInvariant();
        }
    }
}