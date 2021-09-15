using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCryptographyTesting
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
	}
}
