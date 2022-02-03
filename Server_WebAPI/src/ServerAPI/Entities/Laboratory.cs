using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Entities
{
	/// <summary>
	/// Encja Laboratory służy do opisu laboratorium, na które są zapisani studenci.
	/// </summary>
	public class Laboratory
	{

		public Laboratory() { }
		public Laboratory(string labName, string labOrganizer)
		{
			LabName = labName;
			LabOrganizer = labOrganizer;
		}
		/// <summary>
		/// Id laboratorium 
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Nazwa laboratorium
		/// </summary>
		public string LabName { get; set; }

		/// <summary>
		/// Osoba odpowiedzialna za laboratorium
		/// </summary>
		public string LabOrganizer { get; set; }

		public virtual HashSet<LaboratoryRequirement> LaboratoryRequirements { get; set; }
	}
}

