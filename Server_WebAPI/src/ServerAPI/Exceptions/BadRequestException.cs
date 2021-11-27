using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Exceptions
{
	internal class BadRequestException : Exception
	{
		public BadRequestException(string message) : base(message)
		{
		}
	}
}
