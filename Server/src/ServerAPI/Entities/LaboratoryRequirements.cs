using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Entities
{
	public class LaboratoryRequirements
	{
		public LaboratoryRequirements() { }
		public LaboratoryRequirements(string content, DateTime expirationDate, Laboratory laboratory)
		{
			Content = content;
			ExpirationDate = expirationDate;
			Laboratory = laboratory;
		}
		public int Id { get; set; }
		public DateTime ExpirationDate { get; set; }
		public string Content { get; set; }
		public virtual Laboratory Laboratory { get; set; }
	}
}
