using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 文本
	/// </summary>
	public class TextDocument
	{
		private List<TextBlock> _blocks;

		public List<TextBlock> Blocks { get => _blocks; }

		public int BlockCount { get => _blocks.Count; }

		public TextDocument()
		{
			_blocks = new List<TextBlock>();
		}

		public TextDocument(string text)
		{
			_blocks = new List<TextBlock>();
			Init(text);
		}

		private void Init(string text)
		{
			if (!string.IsNullOrEmpty(text))
			{
				string[] stringBlocks = text.Split(new char[] { '\r', '\n' });
				foreach (string sliceText in stringBlocks)
				{
					_blocks.Add(new TextBlock(sliceText));
				}
			}
		}

		/// <summary>
		/// 返回文本
		/// </summary>
		public string PlainText()
		{
			return string.Join("\n", _blocks.Select(x => x.Text).ToArray());
		}

		/// <summary>
		/// 返回文本块
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public TextBlock FindBlockByNumber(int index)
		{
			return _blocks[index];
		}

		/// <summary>
		/// 添加文本块
		/// </summary>
		/// <param name="text"></param>
		public void AppendText(string text)
		{
			_blocks.Add(new TextBlock(text));
		}
	}
}
