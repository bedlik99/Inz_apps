using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StudentAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

namespace StudentAPI
{
	public class Startup
	{
		public IConfiguration Configuration { get; }
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<StudentContext>(opt => opt.UseSqlServer
				(Configuration.GetConnectionString("ServerConnection")));
			services.AddControllers();

			services.AddControllers().AddNewtonsoftJson(s =>
			{
				s.SerializerSettings.ContractResolver = new	CamelCasePropertyNamesContractResolver();
			});

			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

			services.AddScoped<IStudentAPIRepo, SqlStudentAPIRepo>();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
