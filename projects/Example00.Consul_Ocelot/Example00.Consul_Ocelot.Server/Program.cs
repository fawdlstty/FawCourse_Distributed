using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Example00.Consul_Ocelot.Server {
	public class Program {
		private static IConfigurationRoot conf = null;
		public static void Main (string [] args) {
			conf = new ConfigurationBuilder ()
				.SetBasePath (Directory.GetCurrentDirectory ())
				.AddCommandLine (args)
				.AddJsonFile ("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile ($"appsettings.{Environment.GetEnvironmentVariable ("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
				.Build ();
			CreateHostBuilder (args).Build ().Run ();
		}

		public static IHostBuilder CreateHostBuilder (string [] args) =>
			Host.CreateDefaultBuilder (args).ConfigureWebHostDefaults (webBuilder => {
				if (string.IsNullOrEmpty (conf ["port"]))
					throw new Exception ("请通过 --port 命令行参数指定端口");
				Console.WriteLine ($"Listen port: {conf ["port"]}");
				webBuilder.UseUrls ($"http://*:{conf ["port"]}");
				webBuilder.UseKestrel ();
				webBuilder.UseStartup<Startup> ();
			});
	}
}
