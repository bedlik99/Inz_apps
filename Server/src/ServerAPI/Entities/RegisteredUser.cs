using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Entities
{
	public class RegisteredUser
	{
		public RegisteredUser(){}
		public RegisteredUser(string email, string uniqueCode)
		{
			Email = email;
			UniqueCode = uniqueCode;
		}
		public RegisteredUser(string email, string uniqueCode, Laboratory laboratory)
		{
			Email = email;
			UniqueCode = uniqueCode;
			Laboratory = laboratory;
		}
		public int Id { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public string Email { get; set; }
		public string UniqueCode { get; set; }
		public bool NoWarning { get; set; }
		public virtual HashSet<RecordedEvent> EventRegistries { get; set; }
		public virtual Laboratory  Laboratory {get;set;}
	}
}
