using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watson
{
	public class EmptyStackException : WatsonException
	{
		public EmptyStackException() : base()
		{

		}

		public EmptyStackException(string message) : base(message)
		{

		}

		public EmptyStackException(string message, Exception innerException) : base(message, innerException)
		{

		}
	}
}
