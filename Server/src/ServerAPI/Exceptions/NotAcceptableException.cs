using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Exceptions
{
	public class NotAcceptableException : Exception
	{
		public NotAcceptableException(string message) : base(message)
		{

		}
	}
}
