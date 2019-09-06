namespace WebApplication.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using EnsureThat;
    using Microsoft.AspNetCore.Mvc;
    using WebApplication.Core;

    /// <summary>
    /// A class that represent the redirect controller.
    /// </summary>
    public class RedirectController : Controller
    {
        private readonly IRepository repository;

        /// <summary>
        /// Initializes an instance of <see cref="RedirectController">
        /// </summary>
        /// <param name="repository">The repository instance.</param>
        public RedirectController(IRepository repository)
        {
            this.repository = EnsureArg.IsNotNull(repository, nameof(repository));
        }

        /// <summary>
        /// Redirect to the long url.
        /// </summary>
        /// <param name="id">The shorten uri.</param>
        /// <returns>Redirect action to the long url.</returns>
        public async Task<IActionResult> IndexAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var url = await this.repository.GetAsync(id).ConfigureAwait(false);

            if (url == null)
            {
                return NotFound();
            }

            return Redirect(url.ToString());
        }
    }
}