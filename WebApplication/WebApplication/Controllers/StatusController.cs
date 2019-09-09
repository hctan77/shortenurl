using System;
namespace WebApplication.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// A class representing controller for the status api (api/status/).
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        /// <summary>
        /// Get the status of link api.
        /// </summary>
        /// <returns>OK if successful.</returns>
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "OK";
        }
    }
}