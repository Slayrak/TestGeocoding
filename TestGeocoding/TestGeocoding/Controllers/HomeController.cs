using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.IO;
using TestGeocoding.Extentions;
using TestGeocoding.Models;
using static System.Net.Mime.MediaTypeNames;

namespace TestGeocoding.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDistributedCache _distributedCache;

        public HomeController(ILogger<HomeController> logger, IDistributedCache distributedCache)
        {
            _logger = logger;
            _distributedCache = distributedCache;
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
        public async Task<JsonResult> GetLongLat(string street)
        {
            string check = await _distributedCache.GetRecordAsync(street);

            if (string.IsNullOrEmpty(check))
            {
                var innerresult = new System.Net.WebClient().DownloadString(
               $"https://maps.googleapis.com/maps/api/geocode/json?address={street},&key=AIzaSyDUZFWbzmf7Nyf4F3kbX-IdOPhDNYj0l1A");

                await _distributedCache.SetRecordAsync(street, innerresult);

                return Json(innerresult);
            }

            var result = check;

            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetAddress(string latitude, string longitude) 
        {

            string pass = latitude + longitude;

            string check = await _distributedCache.GetRecordAsync(pass);

            if(string.IsNullOrEmpty(check))
            {
                var innerresult = new System.Net.WebClient().DownloadString(
               $"https://maps.googleapis.com/maps/api/geocode/json?latlng={latitude},{longitude}&key=AIzaSyDUZFWbzmf7Nyf4F3kbX-IdOPhDNYj0l1A&location_type=ROOFTOP");

                await _distributedCache.SetRecordAsync(pass, innerresult);

                return Json(innerresult);
            }

            var result = check;

            return Json(result);
        }
    }
}