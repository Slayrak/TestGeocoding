using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.IO;
using TestGeocoding.Models;
using static System.Net.Mime.MediaTypeNames;

namespace TestGeocoding.Controllers
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpGet]
        public IActionResult GetLongLat(string street)
        {
            var result = new System.Net.WebClient().DownloadString(
               $"https://maps.googleapis.com/maps/api/geocode/json?address={street},&key=AIzaSyDUZFWbzmf7Nyf4F3kbX-IdOPhDNYj0l1A");

            return Json(result);
        }

        [HttpGet]
        public IActionResult GetAddress(string latitude, string longitude) 
        {
            var result = new System.Net.WebClient().DownloadString(
               $"https://maps.googleapis.com/maps/api/geocode/json?latlng={latitude},{longitude}&key=AIzaSyDUZFWbzmf7Nyf4F3kbX-IdOPhDNYj0l1A&location_type=ROOFTOP");

            return Json(result);
        }
    }
}