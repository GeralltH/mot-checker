using Microsoft.AspNetCore.Mvc;
using MOTChecker.Models;
using System.Diagnostics;

namespace MOTChecker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AboutMe()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public VehicleModel CheckMot(string registration)
        {
            VehicleModel model = new VehicleModel();
            model.Registration = registration;

            return model;
        }

        [HttpGet]
        public ActionResult Submit(string registration)
        {
            ViewBag.Registration = registration;
            return View("Index");
        }
    }
}