using System;
using RockyToy.Contracts.Common;

namespace RockyToy.Core.Common
{
	public class UserMessageException : Exception, IDoNotLog
	{
		public UserMessageException()
		{

		}
		public UserMessageException(string message, Exception innerException) : base (message, innerException)
		{

		}

		public UserMessageException(string message) : base(message)
		{

		}

	}
}