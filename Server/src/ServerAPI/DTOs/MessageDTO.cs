using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.DTOs
{
	public class MessageDTO
	{
		public MessageDTO() { }

		public string Value { get; set; }
		public override string ToString()
		{
			return "{" + "value='" + Value + '\'' + '}';
		}
	}
}
