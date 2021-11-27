using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.DTOs
{
	public class RegisteredLabUserDTO
	{
		public RegisteredLabUserDTO() { }
		public RegisteredLabUserDTO(string uniqueCode, string email)
		{
			UniqueCode = uniqueCode;
			Email = email;
		}
		public string UniqueCode { get; set; }
		public string Email { get; set; }

		public override string ToString()
		{
			return "{" + "uniqueCode='" + UniqueCode + '\'' +
				 ",email='" + Email + '\'' + '}';
		}
	}
}
