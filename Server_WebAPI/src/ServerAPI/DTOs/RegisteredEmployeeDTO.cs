using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.DTOs
{
	public class RegisteredEmployeeDto
	{
		public string Username { get; set; }
		public string Password { get; set; }
		public string ConfirmedPassword { get; set; }
		public int RoleId { get; set; } = 2;
	}
}