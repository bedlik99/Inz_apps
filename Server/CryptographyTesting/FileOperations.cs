using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCryptographyTesting
{
	class FileOperations
	{
		public FileOperations(string path, string fileName)
		{
			PathToFile = path;
			FileName = fileName;
		}

		string PathToFile { get; set; }
		string FileName { get; set; }

		public void WirteToFile(string stringToWork)
		{
			var filenamePath = Path.Combine(PathToFile, FileName);
			StreamWriter sw;
			if (!File.Exists(filenamePath))
			{
				sw = File.CreateText(filenamePath);
				Console.WriteLine("Utworzono plik txt");
			}
			else
			{
				sw = new StreamWriter(filenamePath, true);
				Console.WriteLine("Otworzono plik txt");
			}
			sw.WriteLine(stringToWork);
			sw.Close();
		}

	}
}
