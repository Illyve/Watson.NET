using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace Watson.Converters
{
	public static class JsonConverter
	{
		public static string Decode(VM vm, string watson)
		{
			vm.Run(watson);
			object x = vm.Pop();
			if (x is not Dictionary<string, object> or List<object>)
			{
				throw new InvalidFormatException("The input is not in a valid json format");
			}
			return JsonConvert.SerializeObject(x);
		}

		public static string Decode(VM vm, StreamReader reader)
		{
			vm.Run(reader);
			object x = vm.Pop();
			if (x is not Dictionary<string, object> or List<object>)
			{
				throw new InvalidFormatException("The input is not in a valid json format");
			}
			return JsonConvert.SerializeObject(x);
		}

		public static string Encode(VM vm, string json)
		{
			object obj = JsonConvert.DeserializeObject(json);
			obj = JsonToWatson(obj);
			return WatsonGen.Generate(obj, vm);
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
