using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServerAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using ServerAPI.Services;
using ServerAPI.Middleware;
using ServerAPI.Settings;

namespace ServerAPI
{
	public class Startup
	{
		//Allows us to access the configuration stored in appsettings.json (read connection string and pass it to DB Context)
		public IConfiguration Configuration { get; }
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}
		public void ConfigureServices(IServiceCollection services)
		{
			//Registration of DB context
			services.AddDbContext<ServerDBContext>(opt => opt.UseSqlServer
				(Configuration.GetConnectionString("ConnectionString")));

			//services are objects that provide funcionality to other parts of the application. Registration of dependencies happens
			services.AddControllers();

			//Registration of dependency (used as Constructor DI)
			services.AddScoped<IEitiLabUserRepo, EitiLabUserService>();

			services.AddControllersWithViews()
				.AddNewtonsoftJson(options =>
				options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
				);

			services.AddScoped<UserSeeder>();

			//Registration for AutoMapper and later DI
			services.AddAutoMapper(this.GetType().Assembly);

			//Adding swagger to the project
			services.AddSwaggerGen();

			//Adding example service as catching Errors
			services.AddScoped<ErrorHandlingMiddleware>();

			//Adding settings class to project
			var crypotgraphySettings = Configuration.GetSection("CryptographySettings");
			services.Configure<CryptographySettings>(crypotgraphySettings);
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserSeeder seeder)
		{
			seeder.Seed();

			//Middlewares
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseMiddleware<ErrorHandlingMiddleware>();
			
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "ServerAPI");
			});

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
