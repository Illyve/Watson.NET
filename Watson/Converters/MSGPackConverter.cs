using System;
using System.IO;

namespace Watson.Converters
{
	internal class MSGPackConverter : Converter
	{
		public override string Decode(string watson, VM vm)
		{
			throw new NotImplementedException();
		}

		public override void Decode(StreamReader reader, StreamWriter writer, VM vm)
		{
			throw new NotImplementedException();
		}

		public override string Encode(string input, VM vm)
		{
			throw new NotImplementedException();
		}

		public override void Encode(StreamReader reader, StreamWriter writer, VM vm)
		{
			throw new NotImplementedException();
		}
	}
}