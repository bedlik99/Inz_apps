using CsvHelper;
using System;
using System.IO;
using System.Linq;

namespace FileOperations
{
	class Program
	{
		static void Main(string[] args)
		{
			using (var streamReader = File.OpenText("some path here"))
			{
				var reader = new CsvReader(streamReader, System.Globalization.CultureInfo.CurrentCulture);
				reader.Context.RegisterClassMap<Class1>();
				var users = reader.GetRecords<RegisteredUser>().ToList();
				foreach (var i in users)
				{
					Console.WriteLine($"{i.Id} {i.IndexNum} {i.UniqueCode}");
				}
			}		
		}
	}
}
