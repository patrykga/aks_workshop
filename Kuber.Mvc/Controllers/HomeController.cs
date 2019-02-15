using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Flurl.Http;
using Kuber.Models;
using Microsoft.AspNetCore.Mvc;
using Kuber.Mvc.Models;

namespace Kuber.Mvc.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var apiModel = await $"http://api.netcoder.com.pl/api/ping".GetJsonAsync<EnvironmentModel>();

            var mvcModel = new EnvironmentModel
            {
                Timestamp = DateTime.Now,
                Hostname = Environment.GetEnvironmentVariable("HOSTNAME")
            };

            return View(new AppEnvironmentModel
            {
                ApiEnvironement = apiModel,
                MvcEnvironement = mvcModel
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
