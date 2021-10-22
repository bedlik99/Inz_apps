using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Entities
{
	public class Laboratory
	{
		public Laboratory(){}
		public Laboratory(string labName, string labOrganizer)
		{
			LabName = labName;
			LabOrganizer = labOrganizer;
		}
		public int Id { get; set; }
		public string LabName { get; set; }
		public string LabOrganizer { get; set; }
		public HashSet<LaboratoryRequirements> LaboratoryRequirements { get; set; }
	}
}
