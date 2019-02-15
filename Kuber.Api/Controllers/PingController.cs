using System;
using Kuber.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kuber.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public EnvironmentModel Get()
        {
            return new EnvironmentModel
            {
                Hostname = Environment.GetEnvironmentVariable("HOSTNAME"),
                Timestamp = DateTime.Now
            };
        }
    }
}
