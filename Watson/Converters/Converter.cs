using System.IO;

namespace Watson.Converters
{
	/// <summary>
	/// <para>
	/// Provides the abstract base class for all watson converters
	/// </para>
	/// </summary>
	public abstract class Converter
	{
		public abstract string Decode(string watson, VM vm);

		public abstract void Decode(StreamReader reader, StreamWriter writer, VM vm);

		public abstract string Encode(string input, VM vm);

		public abstract void Encode(StreamReader reader, StreamWriter writer, VM vm);
	}
}