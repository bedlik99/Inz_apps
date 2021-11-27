using ServerAPI.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CryptographyTesting
{
	class Operations
	{
		//public static string Base64tostring(string base64)
		public static void ComparingTwoApproaches()
		{
			byte[] iv = new byte[]{ (byte)'4', (byte)'q', (byte)'U', (byte)'T',
								(byte)'M', (byte)'L', (byte)'k', (byte)'E',
								(byte)'1', (byte)'5', (byte)'P', (byte)'X',
								(byte)'g', (byte)'6', (byte)'B', (byte)'m' };
			
			byte[] ivv = new byte[] { 52, 113, 85, 84, 77, 76, 107, 69, 49, 53, 80, 88, 103, 54, 66, 109 };

			var base64st = Convert.ToBase64String(iv);
			var base64nd = Convert.ToBase64String(ivv);

			var string1 = Encoding.UTF8.GetString(iv);
			var string2 = Encoding.UTF8.GetString(ivv);
			Console.WriteLine(base64st);
			Console.WriteLine(base64nd);
			
			Console.WriteLine(string1);
			Console.WriteLine(string2);
		}
		public static string RemoveCharsFromString(string str)
		{
			if (str[0] != '{' && str[0] != '[')
			{
				string firstASCIIChar = str.Substring(0, 1);
				int decimalValue = ConvertASCIITo10System(firstASCIIChar);
				str = str.Substring(decimalValue);
			}
			return str;
		}
		
		public static string FillStringWithChars(string str)
		{
			if (str.Length == 0)
				return "";

			long stringNecessarySize = str.Length;

			if (stringNecessarySize % 16 != 0)
				stringNecessarySize = FindMaxRangeOfStringLength(str.Length, 0, 16);


			long numberOfNeededChars = stringNecessarySize - str.Length;
			//Cos sie tu nie zgadza bo jak np. numberOfNeededChars = 5 to doda 5 kolejnych czyli łacznie 6 bedzie.
			///
			/// TO BE CHECKED
			///

			StringBuilder neededChars = new StringBuilder(Cryptography.ConvertASCIITo16System((int)numberOfNeededChars));
			Random random = new Random();

			for (long i = 0; i < numberOfNeededChars-1; i++)
			{
				int randomCharIndex = (int)Math.Floor(random.NextDouble() * 88);
				neededChars.Append(Cryptography.GetWritableChars()[randomCharIndex]);
			}
			if (numberOfNeededChars != 0)
				str = neededChars + str;

			return str;
		}
		public static long FindMaxRangeOfStringLength(long strLength, int lowRange, int highRange)
		{
			if (strLength >= lowRange && strLength < highRange)
			{
				return highRange;
			}
			lowRange += 16;
			highRange += 16;
			return FindMaxRangeOfStringLength(strLength, lowRange, highRange);
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

		public static bool ValidateUserData(string index,string unique)
		{
			Regex regex = new Regex("[0-9]{6}");
			return regex.IsMatch(index) && unique.Length == 6;
		}
	}
}
