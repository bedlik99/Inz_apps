using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Entities
{
	/// <summary>
	/// Encja LaboratoryRequirement służy do opisu wymagania jakie musi zostać spełnione przez studenta w trakcie laboratorium.
	/// </summary>
	public class LaboratoryRequirement
	{
		public LaboratoryRequirement() { }
		public LaboratoryRequirement(string content, Laboratory laboratory)
		{
			Content = content;
			Laboratory = laboratory;
		}

		/// <summary>
		/// Id wymagania laboratoryjnego
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Opis wymagania do spełnienia na laboratorium
		/// </summary>
		public string Content { get; set; }

		public int LaboratoryId { get; set; }
		public virtual Laboratory Laboratory { get; set; }
	}
}

