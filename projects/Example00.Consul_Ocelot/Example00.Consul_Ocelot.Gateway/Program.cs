using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Example00.Consul_Ocelot.Gateway {
	public class Program {
		public static void Main (string [] args) {
			CreateHostBuilder (args).Build ().Run ();
		}

		public static IHostBuilder CreateHostBuilder (string [] args) =>
			Host.CreateDefaultBuilder (args).ConfigureAppConfiguration (conf => {
					conf.AddJsonFile ("ocelot.json", optional: true, reloadOnChange: true);
			}).ConfigureWebHostDefaults (webBuilder => {
				Console.WriteLine ($"Listen port: 6665");
				webBuilder.UseUrls ($"http://*:6665");
				webBuilder.UseKestrel ();
				webBuilder.UseStartup<Startup> ();
			});
	}
}
