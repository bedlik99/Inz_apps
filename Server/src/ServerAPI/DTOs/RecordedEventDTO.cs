using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.DTOs
{
	public class RecordedEventDTO
	{
        public RecordedEventDTO() { }
        public RecordedEventDTO(string indexNr, string registryContent) {
			IndexNr = indexNr;
			RegistryContent = registryContent;
		}
        public string IndexNr { get; set; }
        public string RegistryContent { get; set; }


		public override string ToString()
		{
            return "{" + "indexNr='" + IndexNr + '\'' +
                    ", registryContent='" + RegistryContent + '\'' +
                    '}';
        }
    }
}


  