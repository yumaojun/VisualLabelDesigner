using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiaSharp.SimpleText
{
	/// <summary>
	/// Text Line
	/// </summary>
	public class STTextLine
	{
		public string Text { get; set; }
		public float LineWidth { get; set; }
		public SKPoint Position { get; set; }

		public bool IsValid()
		{
			return Text != null;
		}

		public STTextLine() { }

		public STTextLine(string text)
		{
			Text = text;
		}
	}
}
