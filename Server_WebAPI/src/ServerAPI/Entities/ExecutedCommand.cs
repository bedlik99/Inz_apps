using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Entities
{
	public class ExecutedCommand
	{
		public ExecutedCommand() { }
		public ExecutedCommand(string content,RegisteredUser user, DateTime dateTime) 
		{
			Content=content;
			RegisteredUser = user;
			ExecutionDate = dateTime;
		}
		public int Id { get; set; }
		public string Content { get; set; }
		public DateTime ExecutionDate { get; set; }
		public int RegisteredUserId { get; set; }
		public virtual RegisteredUser RegisteredUser { get; set; }
	}
}
