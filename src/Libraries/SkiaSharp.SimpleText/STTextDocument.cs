using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiaSharp.SimpleText
{
	/// <summary>
	/// Text Document
	/// </summary>
	public class STTextDocument
	{
		private string _text;
		private List<STTextBlock> _blocks;

		public string Text { get => _text; }
		public int BlockCount { get => _blocks.Count; }

		public STTextDocument()
		{
			_text = string.Empty;
			_blocks = new List<STTextBlock>();
		}

		public STTextDocument(string text)
		{
			_text = text;
			_blocks = new List<STTextBlock>();
			InitDocument();
		}

		/// <summary>
		/// 将文本按换行符拆分为一个个Block
		/// </summary>
		private void InitDocument()
		{
			if (!string.IsNullOrEmpty(_text))
			{
				string[] stringBlocks = _text.Split(new char[] { '\r', '\n' });
				foreach (string text in stringBlocks)
				{
					_blocks.Add(new STTextBlock(text));
				}
			}
		}

		public STTextBlock FindBlockByNumber(int index)
		{
			return _blocks[index];
		}
	}
}
