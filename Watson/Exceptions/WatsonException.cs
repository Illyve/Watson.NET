using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
