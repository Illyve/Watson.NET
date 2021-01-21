using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
