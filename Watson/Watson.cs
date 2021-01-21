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
			var vm = new VM(new Lexer(options.InitialMode));

			string input;
			if (options.File == null || !File.Exists(options.File))
			{
				input = Console.ReadLine();
			}
			else
			{
				input = File.ReadAllText(options.File);
			}

			switch (options.Type)
			{
				case "yaml": return YamlConverter.Encode(vm, input);
				case "json": return Converters.JsonConverter.Encode(vm, input);
				default:
					return null;
			}
		}

		public static string Decode(DecodeOptions options)
		{
			var vm = new VM(new Lexer(options.InitialMode));

			string input;
			if (options.Files.Count() == 0)
			{
				input = Console.ReadLine();
			}
			else
			{
				var sb = new StringBuilder();
				foreach (var file in options.Files)
				{
					sb.Append(File.ReadAllText(file));
				}
				input = sb.ToString();
			}

			switch (options.Type)
			{
				case "yaml": return YamlConverter.Decode(vm, input);
				case "json": return Converters.JsonConverter.Decode(vm, input);
				default:
					return null;
			}
		}
	}
}