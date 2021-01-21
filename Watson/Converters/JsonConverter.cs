using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;

namespace Watson.Converters
{
	public static class JsonConverter
	{
		public static string Decode(VM vm, string watson)
		{
			vm.Run(watson);
			object x = vm.Pop();
			if (x is not Dictionary<string, object>)
			{
				throw new InvalidFormatException("The input is not in a valid json format");
			}
			return JsonConvert.SerializeObject(x);
		}

		public static string Encode(VM vm, string json)
		{
			var pairs = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
			return WatsonGen.Generate(pairs, vm.Lexer);
		}
	}
}
