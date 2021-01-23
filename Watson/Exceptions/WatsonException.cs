using System;

namespace Watson
{
	public class WatsonException : Exception
	{
		public WatsonException()
		{
		}

		public WatsonException(string message) : base(message)
		{
		}

		public WatsonException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}