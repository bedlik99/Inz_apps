using System;

namespace CryptographyTesting
{
	public class Program
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
			//string stringToWorkWith = "{\"registryContent\":\"bash_command>ssh -p 8101 karaf@127.0.0.1\",\"uniqueCode\":\"i8`ugD4=\"}";

			//RecordedEvent
			string stringToWorkWith = "{\"registryContent\":\"bash_command>ls\",\"uniqueCode\":\"oUQl,1iv\"}";

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
		public static string EncryptionForTests(string stringToWork)
		{
			stringToWork = Operations.FillStringWithChars(stringToWork);

			//Encrypt
			var encrypt = AES.Encrypt(stringToWork);

			//Decrypt
			var decrypt = AES.Decrypt(encrypt);
			decrypt = Operations.RemoveCharsFromString(decrypt);

			return decrypt;
		}

		public static string EncryptionUserAndEvent(string stringToWorkWith)
		{
			stringToWorkWith = Operations.FillStringWithChars(stringToWorkWith);
			//Encrypt
			var encrypt = AES.Encrypt(stringToWorkWith);
			return encrypt;
		}
	}
}
