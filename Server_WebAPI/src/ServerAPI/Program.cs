using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				})
				.UseNLog();
	}
	//public static IHostBuilder CreateHostBuilder(string[] args) =>
	//	Host.CreateDefaultBuilder(args)
	//		.ConfigureWebHostDefaults(webBuilder =>
	//		{
	//			webBuilder.UseStartup<Startup>();
	//		})
	//		//TBC
	//		//.ConfigureAppConfiguration((context, builder) =>
	//		//{
	//		//	var keyVaultEndpoint = GetKeyVaultEndpoint();
	//		//	if (!string.IsNullOrEmpty(keyVaultEndpoint))
	//		//	{
	//		//		var azureServiceTokenProvider = new AzureServiceTokenProvider();
	//		//		var keyVaultClinet = new KeyVaultClient(
	//		//			new KeyVaultClient.AuthenticationCallback(
	//		//				azureServiceTokenProvider.KeyVaultTokenCallback));
	//		//		builder.AddAzureKeyVault(
	//		//			keyVaultEndpoint, keyVaultClinet, new DefaultKeyVaultSecretManager());

	//		//	}
	//		//})
	//		.UseNLog();

	//private static string GetKeyVaultEndpoint() => "https://labapi.vault.azure.net/";
}
