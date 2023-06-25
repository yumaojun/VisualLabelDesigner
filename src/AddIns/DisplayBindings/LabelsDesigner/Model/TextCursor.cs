using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 文本游标
	/// </summary>
	public class TextCursor
	{
		private TextDocument _doc;

		public TextDocument Doc { get => _doc; }

		public TextCursor(ref TextDocument doc)
		{
			_doc = doc;
		}

		public void InsertBlock()
		{
			// do nothing //_doc.AppendLine("\n");
		}

		public void InsertText(string text)
		{
			_doc.AppendText(text);
		}
	}
}
