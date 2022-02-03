using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ServerAPI.DTOs;
using ServerAPI.DTOs.Validators;
using ServerAPI.Entities;
using ServerAPI.Middleware;
using ServerAPI.Repositories;
using ServerAPI.Services;
using ServerAPI.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerAPI
{
	public class Startup
	{	
		private IConfiguration Configuration { get; }
		//Allows us to access the configuration stored in appsettings.json (read connection string and pass it to DB Context)
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}
		public void ConfigureServices(IServiceCollection services)
		{
			//Registration of DB context
			services.AddDbContext<ServerDBContext>(opt => opt.UseSqlServer
				(Configuration.GetConnectionString("ConnectionString")));

			//Geting info out of appsettings.json
			//Configuring services to have authentication working on JWT
			var authenticationSettings = new AuthenticationSettings();
			Configuration.GetSection("Authentication").Bind(authenticationSettings);
			services.AddSingleton(authenticationSettings);

			var cryptographySettings = new CryptographySettings();
			Configuration.GetSection("Cryptography").Bind(cryptographySettings);
			services.AddSingleton(cryptographySettings);

			var emailSettings = new EmailSettings();
			Configuration.GetSection("Email").Bind(emailSettings);
			services.AddSingleton(emailSettings);

			services.AddAuthentication(option =>
			{
				option.DefaultAuthenticateScheme = "Bearer";
				option.DefaultScheme = "Bearer";
				option.DefaultChallengeScheme = "Bearer";

			}).AddJwtBearer(cfg =>
			{
				cfg.RequireHttpsMetadata = false;
				cfg.SaveToken = true;
				cfg.TokenValidationParameters = new TokenValidationParameters
				{
					ValidIssuer = authenticationSettings.JwtIssuer,
					ValidAudience = authenticationSettings.JwtIssuer,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))
				};
			});

			services.AddAuthorization();

			//services are objects that provide funcionality to other parts of the application. Registration of dependencies happens here.
			services.AddControllers().AddFluentValidation();

			//Registration of dependency (used as Constructor DI)
			services.AddScoped<IEitiLabUserRepo, EitiLabUserService>();
			services.AddScoped<IEmployeeUserRepo, EmployeeUserService>();
			services.AddScoped<IEmailRepo, EmailService>();

			//Registration of DataBase Seeder
			services.AddScoped<DataBaseSeeder>();

			//Registration for AutoMapper and later DI
			services.AddAutoMapper(this.GetType().Assembly);

			//Adding example service as catching Errors
			services.AddScoped<ErrorHandlingMiddleware>();

			//Adding hashing service to project
			//ASP.NET Core Identity Version 3: PBKDF2 with HMAC-SHA256, 128-bit salt, 256-bit subkey, 10000 iterations
			services.AddScoped<IPasswordHasher<Employee>, PasswordHasher<Employee>>();

			//Adding validator to the project
			//Checks value before processing it
			services.AddScoped<IValidator<RegisteredEmployeeDto>, RegisteredEmployeeDTOValidator>();

            //Adding swagger to the project
            services.AddSwaggerGen(config =>
            {
				config.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "WebAPI Server PW",
					Version = "v1",
					Description = "Web application created for WUT " +
					"engineering thesis purposes. API is handling " +
					"requests in system for assessing the independence " +
					"of students' performance of laboratory exercises.",
					Contact = new OpenApiContact
					{
						Name = "Administrator PW",
						Email = "pw.edu.pl",
					}
				});
				var filePath = Path.Combine(System.AppContext.BaseDirectory, "ServerAPI.xml");
				config.IncludeXmlComments(filePath);
			});

			//Function fixing JSON issues
			services.AddControllersWithViews()
					.AddNewtonsoftJson(options =>
					options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

			//CORS settings
			services.AddCors(options =>
				options.AddPolicy(name: "AllowSpecificOrigins",
					builder => builder.AllowAnyOrigin()
			));
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataBaseSeeder seeder)
		{
			//Seeding DB with data if empty
			//seeder.Seed();

			//Middlewares
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			app.UseAuthentication();
			app.UseMiddleware<ErrorHandlingMiddleware>();

			//******************** TESTING for HTML - adding static files **************
			app.UseFileServer(new FileServerOptions
			{
				FileProvider = new PhysicalFileProvider(
				Path.Combine(Directory.GetCurrentDirectory())),
			});

			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "ServerAPI");
				c.RoutePrefix = string.Empty;
			});
			app.UseRouting();
			app.UseCors(builder =>
			{
				builder
				.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader();
			});
			app.UseAuthorization();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
