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
            if (File.Exists("../../../../../../.env"))
            {
                foreach (var line in File.ReadAllLines("../../../../../../.env"))
                {
                    var parts = line.Split(
                        '=',
                        StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length != 2)
                        continue;

                    Environment.SetEnvironmentVariable(parts[0], parts[1]);

                }
            }

            _distributedCache = A.Fake<IDistributedCache>();
            _logger = A.Fake<ILogger<HomeController>>();
            _configuration = new ConfigurationBuilder().AddEnvironmentVariables().Build();

            var check = _configuration["KEY"];

            _homeController = new HomeController(_logger, _distributedCache, _configuration);

            
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
            string inc = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            var result = _homeController.GetLongLat("Kobrynskoi St");

            var checkagain = result.Result.Value as string;

            dynamic data = JObject.Parse(checkagain);

            var name = data.results[0].address_components[0].short_name;

            Assert.True(name.Value == "Kobrynskoi St");
        }
    }
}
