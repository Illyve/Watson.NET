using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watson
{
	public class Lexer
	{

		public enum LexerMode
		{
			A,
			S
		}

		public LexerMode Mode { get; set; }

		public Lexer(LexerMode mode = LexerMode.A)
		{
			Mode = mode;
		}

		public IEnumerable<Operation> GetOperations(string instructions)
		{
			for (int i = 0; i < instructions.Length; i++)
			{
				Operation op = GetOperation(instructions[i]);
				if (op is not null)
				{
					yield return op;
				}
			}
		}

		public IEnumerable<char> GetCharacters(IEnumerable<Operation> operations)
		{
			foreach (Operation op in operations)
			{
				yield return GetChar(op);
			}
		}

		public Operation GetOperation(char letter)
		{
			if (Mode == LexerMode.A)
			{
				return GetOperationA(letter);
			}
			else
			{
				return GetOperationS(letter);
			}
		}

		public void FlipMode()
		{
			if (Mode == LexerMode.A)
			{
				Mode = LexerMode.S;
			}
			else
			{
				Mode = LexerMode.A;
			}
		}

		private Operation GetOperationA(char letter)
		{
			switch (letter)
			{
				case 'B': return Operations.Inew;
				case 'u': return Operations.Iinc;
				case 'b': return Operations.Ishl;
				case 'a': return Operations.Iadd;
				case 'A': return Operations.Ineg;
				case 'e': return Operations.Isht;
				case 'i': return Operations.Itof;
				case '\'': return Operations.Itou;
				case 'q': return Operations.Finf;
				case 't': return Operations.Fnan;
				case 'p': return Operations.Fneg;
				case '?': FlipMode(); return Operations.Snew;
				case '!': return Operations.Sadd;
				case '~': return Operations.Onew;
				case 'M': return Operations.Oadd;
				case '@': return Operations.Anew;
				case 's': return Operations.Aadd;
				case 'z': return Operations.Bnew;
				case 'o': return Operations.Bneg;
				case '.': return Operations.Nnew;
				case 'E': return Operations.Gdup;
				case '#': return Operations.Gpop;
				case '%': return Operations.Gswp;
				default: return null;
			}
		}

		private Operation GetOperationS(char letter)
		{
			switch (letter)
			{
				case 'S': return Operations.Inew;
				case 'h': return Operations.Iinc;
				case 'a': return Operations.Ishl;
				case 'k': return Operations.Iadd;
				case 'r': return Operations.Ineg;
				case 'A': return Operations.Isht;
				case 'z': return Operations.Itof;
				case 'i': return Operations.Itou;
				case 'm': return Operations.Finf;
				case 'b': return Operations.Fnan;
				case 'u': return Operations.Fneg;
				case '$': FlipMode();  return Operations.Snew;
				case '-': return Operations.Sadd;
				case '+': return Operations.Onew;
				case 'g': return Operations.Oadd;
				case 'v': return Operations.Anew;
				case '?': return Operations.Aadd;
				case '^': return Operations.Bnew;
				case '!': return Operations.Bneg;
				case 'y': return Operations.Nnew;
				case '/': return Operations.Gdup;
				case 'e': return Operations.Gpop;
				case ':': return Operations.Gswp;
				default: return null;
			}
		}

		private char GetChar(Operation op)
		{
			switch (Operations.GetString(op))
			{
				case "Inew": return Mode == LexerMode.A ? 'B' : 'S';
				case "Iinc": return Mode == LexerMode.A ? 'u' : 'h';
				case "Ishl": return Mode == LexerMode.A ? 'b' : 'a';
				case "Iadd": return Mode == LexerMode.A ? 'a' : 'k';
				case "Ineg": return Mode == LexerMode.A ? 'A' : 'r';
				case "Isht": return Mode == LexerMode.A ? 'e' : 'A';
				case "Itof": return Mode == LexerMode.A ? 'i' : 'z';
				case "Itou": return Mode == LexerMode.A ? '\'' : 'i';
				case "Finf": return Mode == LexerMode.A ? 'q' : 'm';
				case "Fnan": return Mode == LexerMode.A ? 't' : 'b';
				case "Fneg": return Mode == LexerMode.A ? 'p' : 'u';
				case "Snew": 
					char c = Mode == LexerMode.A ? '?' : '$';
					FlipMode();
					return c;
				case "Sadd": return Mode == LexerMode.A ? '!' : '-';
				case "Onew": return Mode == LexerMode.A ? '~' : '+';
				case "Oadd": return Mode == LexerMode.A ? 'M' : 'g';
				case "Anew": return Mode == LexerMode.A ? '@' : 'v';
				case "Aadd": return Mode == LexerMode.A ? 's' : '?';
				case "Bnew": return Mode == LexerMode.A ? 'z' : '^';
				case "Bneg": return Mode == LexerMode.A ? 'o' : '!';
				case "Nnew": return Mode == LexerMode.A ? '.' : 'y';
				case "Gdup": return Mode == LexerMode.A ? 'E' : '/';
				case "Gpop": return Mode == LexerMode.A ? '#' : 'e';
				case "Gswp": return Mode == LexerMode.A ? '%' : ':';
				default: return ' ';
			}
		}
	}
}
