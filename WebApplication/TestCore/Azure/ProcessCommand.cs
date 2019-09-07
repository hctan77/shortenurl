namespace TestCore.Azure
{
    internal enum ProcessCommand
    {
        /// <summary>
        /// Start the process.
        /// </summary>
        Start,

        /// <summary>
        /// Stop the process.
        /// </summary>
        Stop,

        /// <summary>
        /// Get the process status.
        /// </summary>
        Status,

        /// <summary>
        /// Initialize the process.
        /// </summary>
        Init
    }
}