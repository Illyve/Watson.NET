using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;
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

		[Value(0, MetaName = "file", Required = false, HelpText = "The file to be converted. Will use standard input if not provided")]
		public string File { get; set; }
	}

	[Verb("decode", HelpText = "Converts Watson files into the specified format and outputs it into the standard output")]
	public class DecodeOptions
	{
		[Option('t', "type", Required = true, HelpText = "Specifies the type of the file to be converted")]
		public string Type { get; set; }

		[Option("initial-mode", Required = false, HelpText = "Specifies the initial mode of the lexer", Default = 'A')]
		public char InitialMode { get; set; }

		[Value(0, MetaName = "file", Required = false, HelpText = "The files to be converted, Will use standard input if not provided")]
		public IEnumerable<string> Files { get; set; }
	}

	public class CLI
	{
		public static void Main(string[] args)
		{
			var parser = new CommandLine.Parser(settings => settings.HelpWriter = null);
			var result = parser.ParseArguments<EncodeOptions, DecodeOptions>(args);
			int output = result.MapResult(
				(EncodeOptions options) => { Watson.Encode(options, Console.OpenStandardOutput()); return 0; },
				(DecodeOptions options) => { Watson.Decode(options, Console.OpenStandardOutput()); return 0; },
				(errs) => { DisplayHelp(result); return 1; });
		}

		static void DisplayHelp<T>(ParserResult<T> result)
		{
			var helpText = HelpText.AutoBuild(result, h =>
			{
				h.AdditionalNewLineAfterOption = false;
				h.Heading = "Watson v0.1.0";
				h.Copyright = "";
				return h;
			});

			Console.WriteLine(helpText);
		}
	}
}
