using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Entities
{
	/// <summary>
	/// Encja ExecutedCommand służy do opisu odebranej komendy z maszyny klienckiej dotyczącej spełnienia przez studenta jednego z wymagań  laboratoryjnych.
	/// </summary>
	public class ExecutedCommand
	{
		public ExecutedCommand() { }
		public ExecutedCommand(string content,RegisteredUser user, DateTime dateTime) 
		{
			Content=content;
			RegisteredUser = user;
			ExecutionDate = dateTime;
		}

		/// <summary>
		/// Id odebranej komendy. 
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Opis wykonanej przez studenta komendy na laboratorium
		/// </summary>
		public string Content { get; set; }

		/// <summary>
		/// Data z godziną wykonania komendy
		/// </summary>
		public DateTime ExecutionDate { get; set; }

		public int RegisteredUserId { get; set; }
		public virtual RegisteredUser RegisteredUser { get; set; }
	}
}
