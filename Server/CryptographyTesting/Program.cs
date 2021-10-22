using System;
using System.IO;
using System.Text;
using XCryptographyTesting;

namespace CryptologyTesting
{

	class Program
	{
		static void Main(string[] args)
		{
			Encryption();
			//GenerateUnique();


			//Console.WriteLine(Operations.RemoveCharsFromString("AfLMt'j}bf300480"));
			//string str = "l";
			//var res = Operations.FillStringWithChars(str);
			//Console.WriteLine(res);

			//Console.WriteLine(Operations.ValidateUserData("300480","AASCDED"));
		}

		public static void GenerateUnique()
		{
			var stringCode = AES.GenerateUniqueCode();
			Console.WriteLine(stringCode);
		}

		public static void Encryption()
		{
			var fileOperator = new FileOperations(@"D:\", "plik.txt");
			//Polskie znaki?
			string stringToWorkWith = "{\"email\":\"01143845@pw.edu.pl\",\"uniqueCode\":\"%L`u/hT>\"}";
			//string stringToWorkWith = "{\"email\":\"jakub.wolny.stud@pw.edu.pl\",\"registryContent\":\"Ukonczono zadanie\"}";
			Console.WriteLine(stringToWorkWith);
			stringToWorkWith = Operations.FillStringWithChars(stringToWorkWith);
			Console.WriteLine(stringToWorkWith);
			//Encrypt
			var encrypt = AES.Encrypt(stringToWorkWith);
			Console.WriteLine(encrypt);
			fileOperator.WirteToFile(encrypt);
			//Decrypt
			var decrypt = AES.Decrypt(encrypt);
			Console.WriteLine(decrypt);
		}
	}
}
