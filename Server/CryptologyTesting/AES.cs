using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class AES
{
	private static readonly byte[] byteArrayKey = new byte[] { 109, 90, 113, 52, 116, 55, 119, 33, 122, 37, 67, 42, 70, 45, 74, 64, 78, 99, 82, 102, 85, 106, 88, 110, 50, 114, 53, 117, 56, 120, 47, 65 };

	private static readonly byte[] iv = new byte[]{ (byte)'4', (byte)'q', (byte)'U', (byte)'T',
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
}
