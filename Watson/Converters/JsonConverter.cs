using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Watson.Converters
{
	public class JsonConverter : Converter
	{
		public override string Decode(string watson, VM vm)
		{
			vm.Run(watson);
			object x = vm.Pop();
			if (x is not Dictionary<string, object> or List<object>)
			{
				throw new InvalidFormatException("The input is not in a valid json format");
			}
			return JsonConvert.SerializeObject(x);
		}

		public override void Decode(StreamReader reader, StreamWriter writer, VM vm)
		{
			vm.Run(reader);
			object x = vm.Pop();
			if (x is not Dictionary<string, object> or List<object>)
			{
				throw new InvalidFormatException("The input is not in a valid json format");
			}

			var serializer = JsonSerializer.CreateDefault();
			serializer.Serialize(writer, x);
		}

		public override string Encode(string input, VM vm)
		{
			object obj = JsonConvert.DeserializeObject(input);
			obj = JsonToWatson(obj);
			return WatsonGen.Generate(obj, vm);
		}

		public override void Encode(StreamReader reader, StreamWriter writer, VM vm)
		{
			var serializer = JsonSerializer.CreateDefault();
			object obj = serializer.Deserialize(reader, typeof(object));
			obj = JsonToWatson(obj);
			WatsonGen.Generate(writer, obj, vm);
		}

		private static object JsonToWatson(object obj)
		{
			switch (obj)
			{
				case JObject jsonObj: 
					var wObj = jsonObj.ToObject<Dictionary<string, object>>();
					foreach (var key in wObj.Keys)
					{
						wObj[key] = JsonToWatson(wObj[key]) ?? wObj[key];
					}
					return wObj;
				case JArray jsonArr: 
					var wArr = jsonArr.ToObject<List<object>>();
					for (int i = 0; i < wArr.Count; i++)
					{
						wArr[i] = JsonToWatson(wArr[i]) ?? wArr[i];
					}
					return wArr;
				default: return null;
			}
		}
	}
}
