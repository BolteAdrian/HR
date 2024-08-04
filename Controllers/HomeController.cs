using HR.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace HR.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // GET: Home/Index
        /// <summary>
        /// Displays the home page of the application.
        /// </summary>
        /// <returns>Returns the view for the home page.</returns>
        public IActionResult Index()
        {
            return View();
        }

        // GET: Home/Privacy
        /// <summary>
        /// Displays the privacy policy page of the application.
        /// </summary>
        /// <returns>Returns the view for the privacy policy page.</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        // GET: Home/Error
        /// <summary>
        /// Displays an error page with the request ID for debugging purposes.
        /// </summary>
        /// <returns>Returns the view for the error page.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var errorViewModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(errorViewModel);
        }
    }
}
