using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Example00.Consul_Ocelot.Server.Controllers {
	[Route ("api/[controller]")]
	[ApiController]
	public class HealthController: ControllerBase {
		private readonly ILoggerFactory _loggerFactory;
		public HealthController (ILoggerFactory loggerFactory) {
			_loggerFactory = loggerFactory;
		}

		[HttpGet]
		public ActionResult Get () {
			_loggerFactory.CreateLogger ("Health").LogWarning ("health check call");
			return Ok ("ok");
		}
	}
}
