namespace WebApplication.Controllers.V1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/v1/[controller]")]
    [ApiController]
    public class LinkController : ControllerBase
    {
        [HttpPost]
        public ActionResult<string> Shorten(Uri uri, bool save = true)
        {
            var hash = this.GenerateKey($"{uri.ToString()}{Guid.NewGuid()}");

            return hash;
        }

        private string GenerateKey(string input)
        {
            var hash = CalculateMD5Hash(input);

            // replace / and +
            var base64Hash = Convert.ToBase64String(hash);

            return base64Hash.Replace('/', '_').Replace('+', '-');
        }

        private byte[] CalculateMD5Hash(string input)
        {
            // calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            return md5.ComputeHash(inputBytes);
        }
    }
}