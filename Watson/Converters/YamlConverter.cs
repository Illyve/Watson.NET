using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Core;

namespace Watson.Converters
{
	public static class YamlConverter
	{
		public static string Decode(VM vm, string watson)
		{
			var serializer = new SerializerBuilder().Build();
			vm.Run(watson);
			return serializer.Serialize(vm.Pop()).Trim();
		}

		public static string Encode(VM vm, string yaml)
		{
			var deserializer = new DeserializerBuilder().Build();
			object parsed = deserializer.Deserialize<object>(yaml);
			if (parsed is Dictionary<object, object> pairs)
			{
				parsed = new Dictionary<string, object>(pairs.Select(pair => new KeyValuePair<string, object>((string)pair.Key, pair.Value)));
			}
			return WatsonGen.Generate(parsed, vm.Lexer);
		}
	}
}
