using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Example00.Consul_Ocelot.Server.Controllers {
	[Route ("api/[controller]")]
	[ApiController]
	public class TestController: ControllerBase {
		private readonly ILoggerFactory _loggerFactory;
		private readonly IConfiguration _configuration;
		public TestController (ILoggerFactory loggerFactory, IConfiguration configuration) {
			_loggerFactory = loggerFactory;
			_configuration = configuration;
		}

		[HttpGet]
		public ActionResult Get () {
			_loggerFactory.CreateLogger ("Test").LogWarning ("test service call");
			return Ok ($"hello world, {_configuration["port"]}");
		}
	}
}
