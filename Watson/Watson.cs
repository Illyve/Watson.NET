using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using CommandLine;
using Watson.Converters;
using YamlDotNet.Core;
using Newtonsoft.Json;

namespace Watson
{
	[Verb("encode", HelpText = "Converts a file of the specified format into Watson and outputs it into the standard output")]
	public class EncodeOptions
	{
		[Option('t', "type", Required = true, HelpText = "Specifies the type of the file to be converted")]
		public string Type { get; set; }

		[Option("initial-mode", Required = false, HelpText = "Specifies the initial mode of the lexer", Default = 'A')]
		public char InitialMode { get; set; }

		[Value(0, Required = false, HelpText = "The file to be converted. Will use standard input if not provided")]
		public string File { get; set; }
	}

	[Verb("decode", HelpText = "Converts Watson files into the specified format and outputs it into the standard output")]
	public class DecodeOptions
	{
		[Option('t', "type", Required = true, HelpText = "Specifies the type of the file to be converted")]
		public string Type { get; set; }

		[Option("initial-mode", Required = false, HelpText = "Specifies the initial mode of the lexer", Default = 'A')]
		public char InitialMode { get; set; }

		[Value(0, Required = false, HelpText = "The files to be converted, Will use standard input if not provided")]
		public IEnumerable<string> Files { get; set; }
	}

	public class Watson
	{
		public static void Main(string[] args)
		{
			var result = CommandLine.Parser.Default.ParseArguments<EncodeOptions, DecodeOptions>(args)
				.MapResult(
					(EncodeOptions options) => Encode(options),
					(DecodeOptions options) => Decode(options),
					(errs) => null);

			if (result != null)
			{
				Console.WriteLine(result);
			}
		}

		public static string Encode(EncodeOptions options)
		{
			var vm = new VM();

			string input;
			if (options.File == null || !File.Exists(options.File))
			{
				input = Console.ReadLine();
			}
			else
			{
				input = File.ReadAllText(options.File);
			}

			try
			{
				switch (options.Type)
				{
					case "yaml": return YamlConverter.Encode(vm, input);
					case "json": return Converters.JsonConverter.Encode(vm, input);
					default:
						Console.WriteLine($"Invalid file type");
						return null;
				}
			}
			catch (YamlException ye)
			{
				using (var reader = new StringReader(input))
				{
					string line;
					int lineNumber = 0;
					do
					{
						line = reader.ReadLine();
						lineNumber++;
					}
					while (line != null && lineNumber < ye.Start.Line);
					Console.WriteLine($"Invalid yaml on line {ye.Start.Line} column {ye.Start.Column}: {line}");
				}
				return null;
			}
			catch (JsonSerializationException je)
			{
				using (var reader = new StringReader(input))
				{
					string line;
					int lineNumber = 0;
					do
					{
						line = reader.ReadLine();
						lineNumber++;
					}
					while (line != null && lineNumber < je.LineNumber);
					Console.WriteLine($"Invalid yaml on line {je.LineNumber} column {je.LinePosition}: {line}");
				}
				return null;
			}
			catch (JsonReaderException je)
			{
				using (var reader = new StringReader(input))
				{
					string line;
					int lineNumber = 0;
					do
					{
						line = reader.ReadLine();
						lineNumber++;
					}
					while (line != null && lineNumber < je.LineNumber);
					Console.WriteLine($"Invalid yaml on line {je.LineNumber} column {je.LinePosition}: {line}");
				}
				return null;
			}
		}

		public static string Decode(DecodeOptions options)
		{
			var vm = new VM();

			string input;
			if (options.Files.Count() == 0)
			{
				input = Console.ReadLine();
			}
			else
			{
				input = File.ReadAllText(options.Files.First());
			}

			try
			{
				switch (options.Type)
				{
					case "yaml": return YamlConverter.Decode(vm, input);
					case "json": return Converters.JsonConverter.Decode(vm, input);
					default:
						Console.WriteLine($"Invalid file type");
						return null;
				}
			}
			catch (Exception e) when (e is not WatsonException)
			{
				Console.WriteLine($"Failed to decode watson: {e.Message}");
				return null;
			}
		}
	}
}
