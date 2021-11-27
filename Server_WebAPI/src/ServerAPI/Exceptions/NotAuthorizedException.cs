using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Exceptions
{
	internal class NotAuthorizedException : Exception
	{
		public NotAuthorizedException(string message) : base(message)
		{
		}
	}
}
