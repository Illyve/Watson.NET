using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watson.Converters
{
	static class ConverterFactory
	{
		public static Converter GetConverter(string converterType)
		{
			switch (converterType)
			{
				case "yaml": return new YamlConverter();
				case "json": return new JsonConverter();
				default:
					return null;
			}
		}
	}
}
