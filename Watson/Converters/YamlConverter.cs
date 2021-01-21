using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Core;

namespace Watson.Converters
{
	public class YamlConverter : Converter
	{
		public override string Decode(string watson, VM vm)
		{
			vm.Run(watson);
			var serializer = new SerializerBuilder().Build();
			return serializer.Serialize(vm.Pop()).Trim();
		}

		public override void Decode(StreamReader reader, StreamWriter writer, VM vm)
		{
			vm.Run(reader);
			var serializer = new SerializerBuilder().Build();
			serializer.Serialize(writer, vm.Pop());
		}

		public override string Encode(string input, VM vm)
		{
			var deserializer = new DeserializerBuilder().Build();
			object obj = deserializer.Deserialize<object>(input);
			obj = YamlToWatson(obj);
			return WatsonGen.Generate(obj, vm);
		}

		public override void Encode(StreamReader reader, StreamWriter writer, VM vm)
		{
			var deserializer = new DeserializerBuilder().Build();
			object obj = deserializer.Deserialize<object>(reader);
			obj = YamlToWatson(obj);
			WatsonGen.Generate(writer, obj, vm);
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
