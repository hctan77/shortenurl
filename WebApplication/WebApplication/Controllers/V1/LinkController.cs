namespace WebApplication.Controllers.V1
{
    using System;
    using System.Net;
    using EnsureThat;
    using Microsoft.AspNetCore.Mvc;
    using WebApplication.Core;

    /// <summary>
    /// A class representing controller for the link api (api/v1/link/).
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public sealed class LinkController : ControllerBase
    {
        private const int MaxRetry = 5;
        private readonly IRepository repository;
        private readonly IShortenUrlGenerator shortenUrlGenerator;

        public LinkController(IRepository repository, IShortenUrlGenerator shortenUrlGenerator)
        {
            this.repository = EnsureArg.IsNotNull(repository, nameof(repository));
            this.shortenUrlGenerator = EnsureArg.IsNotNull(shortenUrlGenerator, nameof(shortenUrlGenerator));
        }

        /// <summary>
        /// Shorten the url and store it into repository.
        /// </summary>
        /// <param name="url">The url.</param>
        /// <returns>The shorten url.</returns>
        [HttpPost]
        public ActionResult<string> Shorten(Uri url)
        {
            if (url == null)
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return null;
            }

            int count = 0;
            string shortenUrl = string.Empty;

            for (count = 0; count < MaxRetry; count++)
            {
                shortenUrl = this.shortenUrlGenerator.GetRandomUrl(url);

                if (this.repository.Add(shortenUrl, url))
                {
                    break;
                }
            }

            if (count == MaxRetry)
            {
                this.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return null;
            }

            return shortenUrl;
        }
    }
}