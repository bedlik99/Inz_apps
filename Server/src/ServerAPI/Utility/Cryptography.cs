﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using Microsoft.Extensions.Options;
using ServerAPI.Settings;
using System.Text;

namespace ServerAPI.Utility
{
	public class Cryptography
	{
		// TO BE UNDERSTOOD WHY _cryptographySettings is no usable.

		private readonly CryptographySettings _cryptographySettings;
		public Cryptography(CryptographySettings cryptographySettings)
		{
			_cryptographySettings = cryptographySettings;
		}

		private static readonly string writable_chars = "P_3Auw|g!4EHS1.#W0<+oR?OIZ'9k*6=hCUGtTbQN(f;7/%lr8>LzD2$sy5p@Mq,acBdveKV)nm~ij`Y:&JXF^x-";
				
		//AES256 Key - 256 bit - 32 byte
		private static readonly byte[] byteArrayKey = new byte[] 
										{ 109, 90, 113, 52, 116, 55, 119, 33, 122, 37, 67, 42, 70, 45, 74, 64, 78, 99, 82, 102, 85, 106, 88, 110, 50, 114, 53, 117, 56, 120, 47, 65 };
				
		//IV - Initalization Vector - 128 bit - 16 byte
		private static readonly byte[] iv = new byte[]{ 
											(byte)'4', (byte)'q', (byte)'U', (byte)'T',
											(byte)'M', (byte)'L', (byte)'k', (byte)'E',
											(byte)'1', (byte)'5', (byte)'P', (byte)'X',
											(byte)'g', (byte)'6', (byte)'B', (byte)'m' };
		
		public static string Encrypt(string message)
		{
			string encrypted = null;
			Aes aes = Aes.Create();
			aes.Key = byteArrayKey;
			aes.IV = iv;
			aes.Mode = CipherMode.CBC;
			aes.Padding = PaddingMode.None;

			try
			{
				using var memoryStream = new MemoryStream();
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(byteArrayKey, iv), CryptoStreamMode.Write))
				{
					using StreamWriter streamWriter = new StreamWriter(cryptoStream);
					streamWriter.Write(message);
				}
				byte[] encoded = memoryStream.ToArray();
				encrypted = Convert.ToBase64String(encoded);
			}
			catch (CryptographicException e)
			{
				Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
				return null;
			}
			catch (UnauthorizedAccessException e)
			{
				Console.WriteLine("A file error occurred: {0}", e.Message);
				return null;
			}
			catch (Exception e)
			{
				Console.WriteLine("An error occurred: {0}", e.Message);
				return null;
			}
			finally
			{
				aes.Clear();
			}
			return encrypted;
		}

		public static string Decrypt(string cipherData)
		{
			Aes aes = Aes.Create();
			aes.Key = byteArrayKey;
			aes.IV = iv;
			aes.Mode = CipherMode.CBC;
			aes.Padding = PaddingMode.None;

			try
			{
				using var memoryStream = new MemoryStream(Convert.FromBase64String(cipherData));
				using var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(byteArrayKey, iv), CryptoStreamMode.Read);
				return new StreamReader(cryptoStream).ReadToEnd();
			}
			catch (CryptographicException e)
			{
				Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
				return null;
			}
			catch (UnauthorizedAccessException e)
			{
				Console.WriteLine("A file error occurred: {0}", e.Message);
				return null;
			}
			catch (Exception e)
			{
				Console.WriteLine("An error occurred: {0}", e.Message);
				return null;
			}
			finally
			{
				aes.Clear();
			}
		}

		public static int ConvertASCIITo10System(string valueASCIIChar)
		{
			int value = Convert.ToInt32(valueASCIIChar, 16);

			return value;
		}
		public static string ConvertASCIITo16System(int value)
		{
			int toBase = 16;
			string hex = Convert.ToString(value, toBase);
			return hex;
		}

		public static string GetWritableChars()
		{
			return writable_chars;
		}
		public static string GenerateUniqueCode()
		{
			StringBuilder uniqueCode = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 8; i++)
			{
				int randomCharEmail = (int)Math.Floor(random.NextDouble() * 88);
				uniqueCode.Append(GetWritableChars()[randomCharEmail]);
			}
			return uniqueCode.ToString();
		}
	}
}

