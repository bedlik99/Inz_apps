using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Models
{
	public class RegisteredUser
	{
		public RegisteredUser()	{}
		public RegisteredUser(string indexNum, string uniqueCode)
		{
			IndexNum = indexNum;
			UniqueCode = uniqueCode;
		}
		public int Id { get; set; }
		[Required]
		public string IndexNum { get; set; }
		[Required]
		public string UniqueCode { get; set; }
		public virtual HashSet<RecordedEvent> EventRegistries { get; set; }
	}
}
