using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Watson.Converters
{
	public abstract class Converter
	{
		public abstract string Decode(string watson, VM vm);

		public abstract void Decode(StreamReader reader, StreamWriter writer, VM vm);

		public abstract string Encode(string input, VM vm);

		public abstract void Encode(StreamReader reader, StreamWriter writer, VM vm);
	}
}
