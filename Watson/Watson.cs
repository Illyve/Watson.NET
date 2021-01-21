using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Core;
using Newtonsoft.Json;
using Watson.Converters;

namespace Watson
{
	public static class Watson
	{
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