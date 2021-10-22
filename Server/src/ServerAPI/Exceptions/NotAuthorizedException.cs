using System;
using System.Runtime.Serialization;

namespace ServerAPI.Exceptions
{
	internal class NotAuthorizedException : Exception
	{
		public NotAuthorizedException(string message) : base(message)
		{
		}
	}
}