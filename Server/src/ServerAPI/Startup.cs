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
using Microsoft.AspNetCore.Identity;
using ServerAPI.Entities;
using FluentValidation;
using ServerAPI.DTOs;
using ServerAPI.DTOs.Validators;
using FluentValidation.AspNetCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.FileProviders;
using System.IO;

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

			var cryptogrpahySettings = new CryptographySettings();
			Configuration.GetSection("Cryptography").Bind(cryptogrpahySettings);
			services.AddSingleton(cryptogrpahySettings);

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

			//services are objects that provide funcionality to other parts of the application. Registration of dependencies happens
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

			//Adding hashing service to project (TBD)
			//(TBC) ASP.NET Core Identity Version 3: PBKDF2 with HMAC-SHA256, 128-bit salt, 256-bit subkey, 10000 iterations
			services.AddScoped<IPasswordHasher<Employee>, PasswordHasher<Employee>>();

			//Adding validator to the project
			//Checks value before processing it
			services.AddScoped<IValidator<RegisteredEmployeeDto>, RegisteredEmployeeDTOValidator>();
			
			//Adding swagger to the project
			services.AddSwaggerGen();

			//Function fixing JSON issues
			services.AddControllersWithViews()
					.AddNewtonsoftJson(options =>
					options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
			
			//CORS settings (TBD)
			services.AddCors(options =>
			{
				options.AddDefaultPolicy(
					builder =>
					{
						builder.WithOrigins("https://localhost:44388")
						.AllowAnyHeader()
						.AllowAnyMethod();
					});
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataBaseSeeder seeder)
		{
			//Seeding DB with data if empty
			seeder.Seed();
			//Middlewares
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			app.UseAuthentication();
			app.UseMiddleware<ErrorHandlingMiddleware>();

			//******************** TESTING for HTML **************
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
