using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiaSharp.SimpleText
{
	/// <summary>
	/// Font
	/// </summary>
	public class STFont
	{
		public string Family { get; set; }

		public int PixelSize { get; set; }

		public float PointSize { get; set; }

		public STFontWeight Weight { get; set; }

		public bool Italic { get; set; }

		public bool Underline { get; set; }

		public STFont()
		{

		}

		public STFont(string family, int pointSize = -1, int weight = -1, bool italic = false)
		{

		}
	}

	public static class STFontExtions
	{
		public static SKFont ToSKFont(this STFont that)
		{
			SKTypeface typeface = SKTypeface.FromFamilyName(that.Family);
			return new SKFont(typeface);
		}
	}
}
