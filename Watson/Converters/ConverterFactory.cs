namespace Watson.Converters
{
	internal static class ConverterFactory
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