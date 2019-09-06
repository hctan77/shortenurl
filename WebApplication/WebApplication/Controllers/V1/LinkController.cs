namespace WebApplication.Controllers.V1
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using EnsureThat;
    using Microsoft.AspNetCore.Mvc;
    using WebApplication.Configuration;
    using WebApplication.Core;

    /// <summary>
    /// A class representing controller for the link api (api/v1/link/).
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public sealed class LinkController : ControllerBase
    {
        private readonly IRepository repository;
        private readonly IShortenUrlGenerator shortenUrlGenerator;
        private readonly ApiOptions options;

        public LinkController(IRepository repository, IShortenUrlGenerator shortenUrlGenerator, ApiOptions apiOptions)
        {
            this.repository = EnsureArg.IsNotNull(repository, nameof(repository));
            this.shortenUrlGenerator = EnsureArg.IsNotNull(shortenUrlGenerator, nameof(shortenUrlGenerator));
            this.options = EnsureArg.IsNotNull(apiOptions, nameof(apiOptions));
        }

        /// <summary>
        /// Shorten the url and store it into repository.
        /// </summary>
        /// <param name="url">The url.</param>
        /// <returns>The shorten url.</returns>
        [HttpPost]
        public async Task<ActionResult<string>> ShortenAsync(Uri url)
        {
            if (url == null)
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return null;
            }

            int count = 0;
            string shortenUrl = string.Empty;

            for (count = 0; count < this.options.AddMaxRetry; count++)
            {
                shortenUrl = this.shortenUrlGenerator.GetRandomUrl(url);

                if (await this.repository.AddAsync(shortenUrl, url).ConfigureAwait(false))
                {
                    break;
                }
            }

            if (count == this.options.AddMaxRetry)
            {
                this.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return null;
            }

            return shortenUrl;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Uri>> GetLongUrlAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return null;
            }

            var uri = await this.repository.GetAsync(id).ConfigureAwait(false);

            if (uri == null)
            {
                this.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }

            return uri;
        }
    }
}