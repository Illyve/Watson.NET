using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watson.NET
{


	enum WatsonType
	{
		Int,
		UInt,
		Float,
		String,
		Object,
		Array,
		Bool,
		Nil
	}

	public class VM
	{
		public Lexer Lexer { get; set; } = new Lexer();
		
		private Stack<object> stack = new Stack<object>();

		public void Run(string instructions)
		{
			foreach (Operation op in Lexer.GetOperations(instructions))
			{
				op?.Invoke(this);
			}
		}

		public void Push(object x) => stack.Push(x);

		public object Pop()
		{
			return stack.Pop();
		}

		public T Pop<T>()
		{
			object x = stack.Pop();
			if (x is not T)
			{
				throw new InvalidOperationException($"The top of the stack does not contain the expected value {nameof(T)}");
			}
			return (T)x;
		}
	}
}
