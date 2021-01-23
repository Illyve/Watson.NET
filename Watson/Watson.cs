using System;
using System.IO;
using System.Linq;
using System.Text;
using Watson.Converters;

namespace Watson
{
	/// <summary>
	/// Provides the methods for all Watson processing
	/// </summary>
	public static class Watson
	{
		/// <summary>
		/// <para>Encodes the input to Watson Representation using <paramref name="options"/> and outputs it to the standard output.
		/// Will read from the standard input if the file isn't specified
		/// </para>
		/// </summary>
		/// <param name="options">The encoding options</param>
		/// <returns>A string containing the input's Watson Representation</returns>
		public static string Encode(EncodeOptions options)
		{
			var vm = new VM(options.InitialMode);

			Converter converter = ConverterFactory.GetConverter(options.Type);

			string output;
			if (options.File == null)
			{
				string input = Console.ReadLine();
				output = converter.Encode(input, vm);
			}
			else
			{
				using (var fileStream = new FileStream(options.File, FileMode.Open))
				{
					using (var streamReader = new StreamReader(fileStream))
					{
						output = converter.Encode(streamReader.ReadToEnd(), vm);
					}
				}
			}
			return output;
		}

		/// <summary>
		/// <para>
		/// Encodes the input to Watson Representation using <paramref name="options"/> and stores it in <paramref name="stream"/>
		/// </para>
		/// </summary>
		/// <param name="options">The encoding options</param>
		/// <param name="stream">The output stream</param>
		public static void Encode(EncodeOptions options, Stream stream)
		{
			var vm = new VM(options.InitialMode);

			Converter converter = ConverterFactory.GetConverter(options.Type);

			using (var writer = new StreamWriter(stream))
			{
				if (options.File == null)
				{
					string input = Console.ReadLine();
					writer.Write(converter.Encode(input, vm));
				}
				else
				{
					using (var fileStream = new FileStream(options.File, FileMode.Open))
					{
						using (var reader = new StreamReader(fileStream))
						{
							converter.Encode(reader, writer, vm);
						}
					}
				}
			}
		}

		/// <summary>
		/// <para>
		/// Decodes the input from Watson Representation to the specified format in <paramref name="options"/> and outputs it to the standard output
		/// Will read from the standard input if no files are specified
		/// </para>
		/// </summary>
		/// <param name="options">The decoding options</param>
		/// <returns>A string containing the Watson input in the specified format</returns>
		public static string Decode(DecodeOptions options)
		{
			var vm = new VM(new Lexer(options.InitialMode));

			Converter converter = ConverterFactory.GetConverter(options.Type);

			string input;
			string output = null;

			if (options.Files.Count() == 0)
			{
				input = Console.ReadLine();
				output = converter.Decode(input, vm);
			}
			else
			{
				var sb = new StringBuilder();
				foreach (var file in options.Files)
				{
					using (var fileStream = new FileStream(file, FileMode.Open))
					{
						using (var reader = new StreamReader(fileStream))
						{
							output = converter.Decode(reader.ReadToEnd(), vm);
						}
					}
				}
			}
			return output;
		}

		/// <summary>
		/// <para>
		/// Decodes the input from Watson Representation to the specified format in <paramref name="options"/> and stores it in <paramref name="stream"/>
		/// </para>
		/// </summary>
		/// <param name="options">The decoing options</param>
		/// <param name="stream">The output stream</param>
		public static void Decode(DecodeOptions options, Stream output)
		{
			var vm = new VM(new Lexer(options.InitialMode));

			Converter converter = ConverterFactory.GetConverter(options.Type);

			string input;

			using (var writer = new StreamWriter(output))
			{
				if (options.Files.Count() == 0)
				{
					input = Console.ReadLine();
					writer.Write(converter.Decode(input, vm));
				}
				else
				{
					var sb = new StringBuilder();
					foreach (var file in options.Files)
					{
						using (var fileStream = new FileStream(file, FileMode.Open))
						{
							using (var reader = new StreamReader(fileStream))
							{
								converter.Decode(reader, writer, vm);
							}
						}
					}
				}
			}
		}
	}
}