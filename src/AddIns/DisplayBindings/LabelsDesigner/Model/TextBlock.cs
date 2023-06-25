using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// Text Block
	/// </summary>
	public class TextBlock
	{
		public string Text { get; set; }

		public TextBlock(string text)
		{
			Text = text;
		}
	}
}
