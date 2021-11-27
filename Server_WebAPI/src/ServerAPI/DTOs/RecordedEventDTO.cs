using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.DTOs
{
	public class RecordedEventDTO
	{
		public RecordedEventDTO() { }
		public RecordedEventDTO(string registryContent)
		{
			RegistryContent = registryContent;
		}
		public RecordedEventDTO(string registryContent ,string uniqueCode)
		{
			RegistryContent = registryContent;
			UniqueCode = uniqueCode;
		}

		public string RegistryContent { get; set; }
		public string UniqueCode { get; set; }

		public override string ToString()
		{
			return "{" + "registryContent='" + RegistryContent + '\'' +
					", uniqueCode='" + UniqueCode + '\'' +
					'}';
		}
	}
}
