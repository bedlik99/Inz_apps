﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;

namespace ServerAPI.Extensions
{
	public static class ConfigurationExtensions
	{
		public static void ConfigureKeyVault(this IConfigurationBuilder config)
		{
			string keyVaultEndpoint = Environment.GetEnvironmentVariable("https://labapi.vault.azure.net/");

			if (keyVaultEndpoint is null)
				throw new InvalidOperationException("Store the Key Vault endpoint in a KEYVAULT_ENDPOINT environment variable.");

			//var secretClient = new SecretClient(new(keyVaultEndpoint), new DefaultAzureCredential());
			//config.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());
		}

		public static void WriteConfigurationSources(this IConfigurationBuilder config)
		{
			Console.WriteLine("Configuration sources\n=====================");
			foreach (var source in config.Sources)
			{
				if (source is JsonConfigurationSource jsonSource)
					Console.WriteLine($"{source}: {jsonSource.Path}");
				else
					Console.WriteLine(source.ToString());
			}
			Console.WriteLine("=====================\n");
		}
	}
}