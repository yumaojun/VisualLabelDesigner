using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 文本行
	/// </summary>
	public class TextLine
	{
		public string Text { get; set; }
		public float LineWidth { get; set; }
		public SKPoint Position { get; set; }

		public bool IsValid()
		{
			return Text != null;
		}

		public TextLine() { }

		public TextLine(string text)
		{
			Text = text;
		}

		public SKRect NaturalTextRect()
		{
			return new SKRect();
		}
	}
}
