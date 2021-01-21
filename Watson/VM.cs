using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Watson
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
		public Lexer Lexer { get; set; }
		
		private Stack<object> stack = new Stack<object>();

		public VM()
		{
			Lexer = new Lexer();
		}

		public VM(char mode)
		{
			Lexer = new Lexer(mode);
		}

		public VM(Lexer lexer)
		{
			Lexer = lexer;
		}

		public void Run(string instructions)
		{
			foreach (Operation op in Lexer.GetOperations(instructions))
			{
				op?.Invoke(this);
			}
		}

		public void Run(StreamReader reader)
		{
			foreach (Operation op in Lexer.GetOperations(reader))
			{
				op?.Invoke(this);
			}
		}

		public void Push(object x) => stack.Push(x);

		public object Pop()
		{
			object x;
			if (!stack.TryPop(out x))
			{
				throw new EmptyStackException("The top of the stack is empty");
			}
			return x;
		}

		public T Pop<T>()
		{
			object x = Pop();
			if (x is not T)
			{
				throw new UnexpectedTypeException($"The top of the stack does not contain the expected value {nameof(T)}");
			}
			return (T)x;
		}
	}
}
