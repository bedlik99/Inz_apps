using System;

namespace CryptographyTesting
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

			//RegisteredUser
			//string stringToWorkWith = "{\"email\":\"01143845@pw.edu.pl\",\"uniqueCode\":\"i8`ugD4=\"}";

			//RecordedEvent
			string stringToWorkWith = "{\"registryContent\":\"bash_command>ssh -p 8101 karaf@127.0.0.1\",\"uniqueCode\":\"i8`ugD4=\"}";

			//RecordedEvent
			//string stringToWorkWith = "{\"registryContent\":\"ls\",\"uniqueCode\":\"i8`ugD4=\"}";

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
