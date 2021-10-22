using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Entities
{
	public class Employee
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }	
		//RoleId RACZEJ USELESS - TBC
		public int RoleId { get; set; }
		public virtual Role Role { get; set; }

	}
}
