using System;
using System.Text;
using XCryptographyTesting;

namespace CryptologyTesting
{

	class Program
	{
		static void Main(string[] args)
		{
			//Encrypt
			var encrypt = AES.Encrypt("AfLMt'j}bf300480");
			Console.WriteLine(encrypt);
			//Decrypt
			var decrypt = AES.Decrypt(encrypt);
			Console.WriteLine(decrypt);
		}
	}
}
