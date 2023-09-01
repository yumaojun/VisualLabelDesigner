using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// Raw Text Type 原始文本类型
	/// </summary>
	public struct RawText
	{
		/// <summary>
		/// 标志
		/// </summary>
		struct Token
		{
			public bool IsField { get; set; }
			public string Text { get; set; }
			public SubstitutionField Field { get; set; }
		};

		private string _string;
		private List<Token> _tokens;

		/// <summary>
		/// Constructor from string 从字符串构造函数
		/// </summary>
		public RawText(string str)
		{
			_string = str;
			_tokens = new List<Token>();
			Tokenize();
		}

		/// <summary>
		/// Tokenize string 标记化字符串
		/// </summary>
		private void Tokenize()
		{
			Token token = new Token();
			string refString = _string;

			while (refString.Length > 0)
			{
				SubstitutionField field = new SubstitutionField();
				if (SubstitutionField.Parse(ref refString, field))
				{
					// Finalize current text token, if apropos 完成当前文本标记（如果合适）
					if (!string.IsNullOrEmpty(token.Text))
					{
						token.IsField = false;
						_tokens.Add(token);
					}

					// Create and finalize field token 创建并完成字段令牌
					token.IsField = true;
					token.Text = string.Empty;
					token.Field = field;
					_tokens.Add(token);
				}
				else
				{
					token.Text += refString[0];
					refString = refString.Substring(1);
				}
			}

			// Finalize last text token, if apropos
			if (!string.IsNullOrEmpty(token.Text))
			{
				token.IsField = false;
				_tokens.Add(token);
			}
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
