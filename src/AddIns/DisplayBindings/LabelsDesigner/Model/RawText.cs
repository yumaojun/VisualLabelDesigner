using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	struct Token
	{
		public bool IsField { get; set; }
		public string Text { get; set; }
		public SubstitutionField Field { get; set; }
	};

	public struct RawText
	{
		private string _string;
		private List<Token> _tokens;

		///
		/// Constructor from string
		///
		public RawText(string str)
		{
			_string = str;
			_tokens = new List<Token>();
			Tokenize();
		}

		/////
		///// Constructor from C string operator
		/////
		//RawText(  char* cString ) : mString(QString(cString))
		//{
		//	Tokenize();
		//}

		private void Tokenize()
		{

		}

		/// <summary>
		/// Is raw text empty?
		/// </summary>
		public bool IsEmpty()
		{
			return string.IsNullOrEmpty(_string);
		}

		/// <summary>
		/// Return Text
		/// </summary>
		public override string ToString()
		{
			return _string?.ToString();
		}

		/// Expand all place holders
		public string Expand(Backends.Merge.Record record, Variables variables)
		{
			string text = string.Empty;

			foreach (Token token in _tokens)
			{
				if (token.IsField)
				{
					text += token.Field.Evaluate(record, variables);
				}
				else
				{
					text += token.Text;
				}
			}

			return text;
		}

		public bool HasPlaceHolders()
		{
			return System.Text.RegularExpressions.Regex.IsMatch(_string, "\\${\\w+}");
		}

		public static implicit operator RawText(string str) => new RawText(str);
		public static implicit operator string(RawText rawText) => rawText.ToString();
	}
}
