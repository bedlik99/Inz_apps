using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.DTOs
{
	public class RecordedEventDTO
	{
		public RecordedEventDTO() { }
		public RecordedEventDTO(string email, string registryContent)
		{
			Email = email;
			RegistryContent = registryContent;
		}
		public string Email { get; set; }
		public string RegistryContent { get; set; }

		public override string ToString()
		{
			return "{" + "email='" + Email + '\'' +
					", registryContent='" + RegistryContent + '\'' +
					'}';
		}
	}
}


