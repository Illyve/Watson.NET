using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watson
{
	public class InvalidFormatException : WatsonException
	{
		public InvalidFormatException() : base()
		{

		}

		public InvalidFormatException(string message) : base(message)
		{

		}

		public InvalidFormatException(string message, Exception innerException) : base(message, innerException)
		{

		}
	}
}
