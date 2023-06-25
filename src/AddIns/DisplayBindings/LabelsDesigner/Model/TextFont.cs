using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 文本字体属性
	/// </summary>
	public struct TextFont
	{
		public string Family { get; set; }
		public float PointSize { get; set; }
		public SKFontStyleWeight Weight { get; set; }
		public bool Italic { get; set; }
		public bool Underline { get; set; }
	}
}
