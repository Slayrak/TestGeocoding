using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGeocoding.Controllers;
using DotNetEnv;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TestGeocoding.Tests
{
    public class HomeControllerTests
    {
        private ILogger<HomeController> _logger;
        private IDistributedCache _distributedCache;
        private IConfiguration _configuration;
        private HomeController _homeController;
        public HomeControllerTests()
        {

           var checker = Environment.GetEnvironmentVariable("API_KEY");

            _distributedCache = A.Fake<IDistributedCache>();
            _logger = A.Fake<ILogger<HomeController>>();

            _homeController = new HomeController(_logger, _distributedCache);

            
        }

        [Fact]
        public void HomeController_Index_isNotNull()
        {
            var result = _homeController.Index() as ViewResult;

            Assert.NotNull(result);

        }

        [Fact]
        public void HomeController_GetLongLat_isNotNull() 
        {
            var result = _homeController.GetLongLat("Kobrynskoi St");

            var checkagain = result.Result.Value as string;

            dynamic data = JObject.Parse(checkagain);

            var name = data.results[0].address_components[0].short_name;

            Assert.True(name.Value == "Kobrynskoi St");
        }

        [Fact]
        public void HomeController_GetAddress_isNotNull()
        {
            var result = _homeController.GetAddress("49.8623618", "23.9967532");

            var checkagain = result.Result.Value as string;

            dynamic data = JObject.Parse(checkagain);

            var name = data.results[0].address_components[1].short_name;

            Assert.True(name.Value == "Vynnytsya St");
        }
    }
}
