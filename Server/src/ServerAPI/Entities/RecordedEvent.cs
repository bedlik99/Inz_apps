using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Entities
{
	public class RecordedEvent
	{
		public RecordedEvent() {}
		public RecordedEvent(string registryContent, DateTime dateTime, RegisteredUser registeredUser)
		{
			RegistryContent = registryContent;
			DateTime = dateTime;
			RegisteredUser = registeredUser;
		}
		public int Id { get; set; }
		public string RegistryContent { get; set; }
		public DateTime DateTime { get; set; }
		public int RegisteredUserId { get; set; }
		public virtual RegisteredUser RegisteredUser { get; set; }
	}
}
