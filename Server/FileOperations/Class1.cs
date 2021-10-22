using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOperations
{
	sealed class Class1 : ClassMap<RegisteredUser>
	{
		public Class1()
		{
			Map(m => m.Id).Index(0);
			Map(m => m.IndexNum).Index(1);
			Map(m => m.UniqueCode).Index(2);
		}
	}
}
