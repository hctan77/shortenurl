using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Configuration
{
    /// <summary>
    /// Represents the setting data for API.
    /// </summary>
    public sealed class ApiOptions
    {
        /// <summary>
        /// Maximum retry for add.
        /// </summary>
        public int AddMaxRetry { get; set; }

        /// <summary>
        /// Base address for link api.
        /// </summary>
        public string LinkApiBaseAddress { get; set; }
    }
}
