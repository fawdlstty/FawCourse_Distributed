using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Example00.Consul_Ocelot.Server {
	public class Startup {
		public Startup (IConfiguration configuration) {
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices (IServiceCollection services) {
			services.AddControllers ();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure (IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory) {
			if (env.IsDevelopment ()) {
				app.UseDeveloperExceptionPage ();
			}

			app.UseRouting ();

			app.UseAuthorization ();

			app.UseEndpoints (endpoints => {
				endpoints.MapControllers ();
			});

			loggerFactory.CreateLogger ("Startup").LogWarning (Configuration ["port"]);
			using (ConsulClient client = new ConsulClient (c => {
				c.Address = new Uri ("http://127.0.0.1:8500/");
				c.Datacenter = "dc1";
			})) {
				client.Agent.ServiceRegister (new AgentServiceRegistration {
					ID = $"test_service_{Guid.NewGuid ().ToString ()}",
					Name = "test_service",
					Address = "127.0.0.1",
					Port = int.Parse (Configuration ["port"]),
					Tags = new string [] { },
					Check = new AgentServiceCheck {
						DeregisterCriticalServiceAfter = TimeSpan.FromSeconds (1),
						Interval = TimeSpan.FromSeconds (10),
						Timeout = TimeSpan.FromSeconds (1),
						HTTP = $"http://127.0.0.1:{Configuration ["port"]}/api/Health"
					}
				});
			}
		}
	}
}
