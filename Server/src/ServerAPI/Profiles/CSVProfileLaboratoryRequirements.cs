using CsvHelper.Configuration;
using ServerAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Profiles
{
	public class CSVProfileLaboratoryRequirements : DefaultClassMap<LaboratoryRequirements>
	{
		public CSVProfileLaboratoryRequirements()
		{
			Map(m => m.Content).Name("Requirement");
			Map(m => m.ExpirationDate).Name("ExpirationDate");
		}
	}
}
