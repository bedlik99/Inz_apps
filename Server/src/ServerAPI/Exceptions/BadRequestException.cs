using System;
using System.Runtime.Serialization;

namespace ServerAPI.Exceptions
{
	internal class BadRequestException : Exception
	{
		public BadRequestException(string message) : base(message)
		{
		}
	}
}