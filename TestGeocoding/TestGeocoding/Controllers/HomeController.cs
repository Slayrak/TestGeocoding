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
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IDistributedCache distributedCache, IConfiguration configuration)
        {
            _logger = logger;
            _distributedCache = distributedCache;
            _configuration = configuration;

        }

        public IActionResult Index()
        {
            _logger.LogInformation("Accessed the main page", DateTime.UtcNow);
            return View();
        }

        public IActionResult Privacy()
        {
            _logger.LogInformation("Redirected to the privacy page", DateTime.UtcNow);
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
            string check = "";

            try
            {
                check = await _distributedCache.GetRecordAsync(street);
            } catch (Exception ex)
            {
                _logger.LogError("No connection to the database", DateTime.UtcNow);
            }

            if (string.IsNullOrEmpty(check))
            {
                _logger.LogInformation("Cache did not find entry with parameter " + street, DateTime.UtcNow);
                _logger.LogInformation("Geocoding with parameter " + street, DateTime.UtcNow);

                var innerresult = new System.Net.WebClient().DownloadString(
               $"https://maps.googleapis.com/maps/api/geocode/json?address={street},&key={_configuration["KEY"]}");

                _logger.LogInformation("Retrieved information from google api with parameter " + street, DateTime.UtcNow);

                try
                {
                    await _distributedCache.SetRecordAsync(street, innerresult);
                    _logger.LogInformation("Successfully cached information", DateTime.UtcNow);

                } catch (Exception ex)
                {
                    _logger.LogError("No connection to the database", DateTime.UtcNow);
                }
                
                return Json(innerresult);
            }

            _logger.LogInformation("Cache found entry with parameter " + street, DateTime.UtcNow);
            var result = check;

            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetAddress(string latitude, string longitude) 
        {

            string pass = latitude + longitude;

            string check = "";

            try
            {
                check = await _distributedCache.GetRecordAsync(pass);
            } catch (Exception ex)
            {
                _logger.LogError("No connection to the database", DateTime.UtcNow);
            }


            if(string.IsNullOrEmpty(check))
            {
                _logger.LogInformation("Cache did not find entry with latitude: " + latitude + " and longitude: " + longitude, DateTime.UtcNow);
                _logger.LogInformation("Geocoding with latitude: " + latitude + " and longitude: " + longitude, DateTime.UtcNow);

                var innerresult = new System.Net.WebClient().DownloadString(
               $"https://maps.googleapis.com/maps/api/geocode/json?latlng={latitude},{longitude}&key={_configuration["KEY"]}&location_type=ROOFTOP");

                _logger.LogInformation("Retrieved information from google api with: " + latitude + " and longitude: " + longitude, DateTime.UtcNow);

                try
                {
                    await _distributedCache.SetRecordAsync(pass, innerresult);
                    _logger.LogInformation("Successfully cached information", DateTime.UtcNow);
                } catch (Exception ex)
                {
                    _logger.LogError("No connection to the database", DateTime.UtcNow);
                }
                

                return Json(innerresult);
            }

            _logger.LogInformation("Cache found entry with latitude: " + latitude + " and longitude: " + longitude, DateTime.UtcNow);
            var result = check;

            return Json(result);
        }
    }
}