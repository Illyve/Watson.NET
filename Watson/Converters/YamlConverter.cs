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

		public static string Decode(VM vm, StreamReader reader)
		{
			var serializer = new SerializerBuilder().Build();
			vm.Run(reader);
			return serializer.Serialize(vm.Pop()).Trim();
		}

		public static string Encode(VM vm, string yaml)
		{
			var deserializer = new DeserializerBuilder().Build();
			object obj = deserializer.Deserialize<object>(yaml);
			obj = YamlToWatson(obj);
			return WatsonGen.Generate(obj, vm);
		}

		private static object YamlToWatson(object obj)
		{
			if (obj is Dictionary<object, object> yamlObj)
			{
				//Converts all the keys to the strings in the obj and all its children obj
				return new Dictionary<string, object>(
					yamlObj
						.Select(pair => new KeyValuePair<string, object>((string)pair.Key, YamlToWatson(pair.Value)))
				);
			}
			return null;
		}
	}
}
