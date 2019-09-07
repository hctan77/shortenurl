namespace WebApplication.Controllers
{
    using System.Diagnostics;
    using System.Net;
    using Microsoft.AspNetCore.Mvc;
    using WebApplication.Models;

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}