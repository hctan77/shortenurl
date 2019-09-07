namespace TestCore.Azure
{
    using System;
    using System.Diagnostics;
    using System.Linq;

    public static class AzureStorageEmulatorManager
    {
        public static bool IsProcessRunning()
        {
            bool status;

            using (var process = Process.Start(AzureStorageEmulatorProcessFactory.Create(ProcessCommand.Status)))
            {
                if (process == null)
                {
                    throw new InvalidOperationException("Unable to start process.");
                }

                status = GetStatus(process);
                process.WaitForExit();
            }

            return status;
        }

        public static void StartStorageEmulator()
        {
            if (!IsProcessRunning())
            {
                ExecuteProcess(ProcessCommand.Init);
                ExecuteProcess(ProcessCommand.Start);
            }
        }

        public static void StopStorageEmulator()
        {
            if (IsProcessRunning())
            {
                ExecuteProcess(ProcessCommand.Stop);
            }
        }

        private static void ExecuteProcess(ProcessCommand command)
        {
            string error;

            using (var process = Process.Start(AzureStorageEmulatorProcessFactory.Create(command)))
            {
                if (process == null)
                {
                    throw new InvalidOperationException("Unable to start process.");
                }

                error = GetError(process);
                process.WaitForExit();
            }

            if (!string.IsNullOrEmpty(error))
            {
                throw new InvalidOperationException(error);
            }
        }

        private static string GetError(Process process)
        {
            string output = process.StandardError.ReadToEnd();
            return output.Split(':').Select(part => part.Trim()).Last();
        }

        private static bool GetStatus(Process process)
        {
            string output = process.StandardOutput.ReadToEnd();
            string isRunningLine = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).SingleOrDefault(line => line.StartsWith("IsRunning", StringComparison.OrdinalIgnoreCase));

            if (isRunningLine == null)
            {
                return false;
            }

            return bool.Parse(isRunningLine.Split(':').Select(part => part.Trim()).Last());
        }
    }
}