using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Entities
{
	/// <summary>
	/// Encja RecordedEvent służy do opisu odebranej informacji z maszyny klienckiej opisującej wykonaną czynność studenta w systemie na laboratorium.
	/// </summary>
	public class RecordedEvent
	{
		public RecordedEvent() { }
		public RecordedEvent(string registryContent, DateTime dateTime, RegisteredUser registeredUser)
		{
			RegistryContent = registryContent;
			DateTime = dateTime;
			RegisteredUser = registeredUser;
		}
		/// <summary>
		/// Id odebranego zdarzenia
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Opis wykonanego przez studenta zdarzenia na laboratorium
		/// </summary>
		public string RegistryContent { get; set; }

		/// <summary>
		/// Data z godziną otrzymanego na serwerze zdarzenia
		/// </summary>
		public DateTime DateTime { get; set; }

		public int RegisteredUserId { get; set; }
		public virtual RegisteredUser RegisteredUser { get; set; }
	}
}

