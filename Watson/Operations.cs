using System;
using System.Collections.Generic;

namespace Watson
{
	public delegate void Operation(VM vm);

	public static class Operations
	{
		public static Operation Inew => vm => vm.Push((long)0);

		public static Operation Iinc => vm =>
		{
			long x = vm.Pop<long>();
			vm.Push(x + 1);
		};

		public static Operation Ishl => vm =>
		{
			long x = vm.Pop<long>();
			vm.Push(x << 1);
		};

		public static Operation Iadd => vm =>
		{
			long y = vm.Pop<long>();
			long x = vm.Pop<long>();
			vm.Push(x + y);
		};

		public static Operation Ineg => vm =>
		{
			long x = vm.Pop<long>();
			vm.Push(-x);
		};

		public static Operation Isht => vm =>
		{
			long y = vm.Pop<long>();
			long x = vm.Pop<long>();
			vm.Push(x << (int)y);
		};

		public static Operation Itof => vm =>
		{
			long x = vm.Pop<long>();
			byte[] xBytes = BitConverter.GetBytes(x);
			double y = BitConverter.ToDouble(xBytes);
			vm.Push(y);
		};

		public static Operation Itou => vm =>
		{
			long x = vm.Pop<long>();
			byte[] xBytes = BitConverter.GetBytes(x);
			ulong y = BitConverter.ToUInt64(xBytes);
			vm.Push(y);
		};

		public static Operation Finf => vm =>
		{
			vm.Push(double.PositiveInfinity);
		};

		public static Operation Fnan => vm =>
		{
			vm.Push(double.NaN);
		};

		public static Operation Fneg => vm =>
		{
			double x = vm.Pop<double>();
			vm.Push(-x);
		};

		public static Operation Snew => vm =>
		{
			vm.Push("");
		};

		public static Operation Sadd => vm =>
		{
			long x = vm.Pop<long>();
			string s = vm.Pop<string>();
			s += (char)(x & 0xFF);
			vm.Push(s);
		};

		public static Operation Onew => vm =>
		{
			vm.Push(new Dictionary<string, object>());
		};

		public static Operation Oadd => vm =>
		{
			object v = vm.Pop();
			string k = vm.Pop<string>();
			Dictionary<string, object> o = vm.Pop<Dictionary<string, object>>();
			o[k] = v;
			vm.Push(o);
		};

		public static Operation Anew => vm =>
		{
			vm.Push(new List<object>());
		};

		public static Operation Aadd => vm =>
		{
			object x = vm.Pop<object>();
			List<object> a = vm.Pop<List<object>>();
			a.Add(x);
			vm.Push(a);
		};

		public static Operation Bnew => vm =>
		{
			vm.Push(false);
		};

		public static Operation Bneg => vm =>
		{
			bool x = vm.Pop<bool>();
			vm.Push(!x);
		};

		public static Operation Nnew => vm =>
		{
			vm.Push(null);
		};

		public static Operation Gdup => vm =>
		{
			object x = vm.Pop<object>();
			vm.Push(x);
			vm.Push(x);
		};

		public static Operation Gpop => vm =>
		{
			vm.Pop<object>();
		};

		public static Operation Gswp => vm =>
		{
			object y = vm.Pop<object>();
			object x = vm.Pop<object>();
			vm.Push(y);
			vm.Push(x);
		};

		public static string GetString(Operation op) => operationTable[op];

		private static Dictionary<Operation, string> operationTable = new Dictionary<Operation, string>()
		{
			{ Inew, "Inew" },
			{ Iinc, "Iinc" },
			{ Ishl, "Ishl" },
			{ Iadd, "Iadd" },
			{ Ineg, "Ineg" },
			{ Isht, "Isht" },
			{ Itof, "Itof" },
			{ Itou, "Itou" },
			{ Finf, "Finf" },
			{ Fnan, "Fnan" },
			{ Fneg, "Fneg" },
			{ Snew, "Snew" },
			{ Sadd, "Sadd" },
			{ Onew, "Onew" },
			{ Oadd, "Oadd" },
			{ Anew, "Anew" },
			{ Aadd, "Aadd" },
			{ Bnew, "Bnew" },
			{ Bneg, "Bneg" },
			{ Nnew, "Nnew" },
			{ Gdup, "Gdup" },
			{ Gpop, "Gpop" },
			{ Gswp, "Gswp" },
		};
	}
}