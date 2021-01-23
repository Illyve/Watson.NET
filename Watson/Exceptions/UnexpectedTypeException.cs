using System;

namespace Watson
{
	public class UnexpectedTypeException : WatsonException
	{
		public UnexpectedTypeException() : base()
		{
		}

		public UnexpectedTypeException(string message) : base(message)
		{
		}

		public UnexpectedTypeException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}