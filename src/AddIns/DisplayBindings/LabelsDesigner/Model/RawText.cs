using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	struct Token
	{
		bool isField;
		string text;
		SubstitutionField field;
	};

	public struct RawText
	{
		private string _string;
		private List<Token> _tokens;

		///
		/// Constructor from string
		///
		public RawText( string str ) 
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

		public override string ToString()
		{
			return _string.ToString();
		}

		public static implicit operator RawText(string str) => new RawText(str);
		public static implicit operator string(RawText rawText) => rawText.ToString();
	}
}
