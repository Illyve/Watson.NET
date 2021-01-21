using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Watson
{
	static class WatsonGen
	{
		public static string Generate(object value) => Generate(value, new VM());
		public static string Generate(object value, VM vm)
		{
			IEnumerable<Operation> ops = GenerateAny(value);
			IEnumerable<char> chars = vm.Lexer.GetCharacters(ops);
			return new String(chars.ToArray());
		}

		public static IEnumerable<Operation> GenerateAny(object value)
		{
			switch (value)
			{
				case int val: return GenerateInt(val);
				case long val: return GenerateInt(val);
				case uint val: return GenerateUInt(val);
				case ulong val: return GenerateUInt(val);
				case double val: return GenerateFloat(val);
				case string val: return GenerateString(val);
				case Dictionary<string, object> val: return GenerateObject(val);
				case List<object> val: return GenerateArray(val);
				case bool val: return GenerateBool(val);
				case null: return GenerateNil();
				default: 
					return new List<Operation>();
			}
		}

		private static IEnumerable<Operation> GenerateInt(long value)
		{
			var ops = new List<Operation>();
			if (value == 0)
			{
				ops.Add(Operations.Inew);
			}
			else
			{
				// Starts the number
				ops.Add(Operations.Inew);
				ops.Add(Operations.Iinc);

				int MSBBit = (int)Math.Log2(value);
				for (int offset = MSBBit - 1; offset >= 0; offset--) // Already counted the 1st bit
				{
					ops.Add(Operations.Ishl);
					if ((value & (1 << offset)) != 0)
					{
						ops.Add(Operations.Iinc);
					}
				}
			}
			return ops;
		}

		private static IEnumerable<Operation> GenerateUInt(ulong value)
		{
			var ops = new List<Operation>();
			if (value == 0)
			{
				ops.Add(Operations.Inew);
			}
			else
			{
				ops.Add(Operations.Inew);
				ops.Add(Operations.Iinc);

				int MSBBit = (int)Math.Log2(value);
				for (int offset = MSBBit - 1; offset >= 0; offset--)
				{
					ops.Add(Operations.Ishl);
					if ((value & (ulong)(1 << offset)) == 1)
					{
						ops.Add(Operations.Iinc);
					}
				}
			}
			ops.Add(Operations.Itou);
			return ops;
		}

		private static IEnumerable<Operation> GenerateFloat(double value)
		{
			var ops = new List<Operation>();
			byte[] bytes = BitConverter.GetBytes(value);
			for (int i = 0; i < bytes.Length; i++)
			{
				ops.AddRange(GenerateInt(bytes[i]));
			}
			ops.Add(Operations.Itof);
			return ops;
		}

		private static IEnumerable<Operation> GenerateString(string value)
		{
			var ops = new List<Operation>();
			byte[] bytes = Encoding.ASCII.GetBytes(value);
			ops.Add(Operations.Snew);
			for (int i = 0; i < bytes.Length; i++)
			{
				ops.AddRange(GenerateInt(bytes[i]));
				ops.Add(Operations.Sadd);
			}
			return ops;
		}

		private static IEnumerable<Operation> GenerateObject(Dictionary<string, object> value)
		{
			var ops = new List<Operation>();
			ops.Add(Operations.Onew);
			foreach (var key in value.Keys)
			{
				ops.AddRange(GenerateString(key));
				ops.AddRange(GenerateAny(value[key]));
				ops.Add(Operations.Oadd);
			}
			return ops;
		}

		private static IEnumerable<Operation> GenerateArray(List<object> value)
		{
			var ops = new List<Operation>();
			ops.Add(Operations.Anew);
			for (int i = 0; i < value.Count; i++)
			{
				ops.AddRange(GenerateAny(value[i]));
				ops.Add(Operations.Aadd);
			}
			return ops;
		}

		private static IEnumerable<Operation> GenerateBool(bool value)
		{
			var ops = new List<Operation>();
			ops.Add(Operations.Bnew);
			if (value)
			{
				ops.Add(Operations.Bneg);
			}
			return ops;
		}

		private static IEnumerable<Operation> GenerateNil()
		{
			var ops = new List<Operation>();
			ops.Add(Operations.Nnew);
			return ops;
		}
	}
}
