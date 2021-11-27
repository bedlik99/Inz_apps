using CsvHelper.Configuration;
using ServerAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Profiles
{
	public class CSVProfileUser : DefaultClassMap<RegisteredUser>
	{
		public CSVProfileUser()
		{
			Map(m => m.Email).Name("Mail");
		}
	}
}
