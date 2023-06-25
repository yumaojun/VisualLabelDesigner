using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 文本布局
	/// </summary>
	public class TextLayout
	{
		private int _textCount;
		private string _text;
		private List<TextLine> _textLines;

		public string Text { get => _text; }

		public TextLayout(string text)
		{
			_text = text;
			_textLines = new List<TextLine>();
			_textCount = text.Length;
		}

		public int LineCount { get => _textLines.Count; }

		public SKRect Bounds { get; }

		public float FontSpacing
		{
			get
			{
				float textSize = Labels.CommonHelper.PhysicalDpiX() * TextFont.PointSize / Constants.PTS_PER_INCH;
				SKFontStyleSlant slant = TextFont.Italic ? SKFontStyleSlant.Italic : SKFontStyleSlant.Upright;

				using (SKFontStyle fontStyle = new SKFontStyle(TextFont.Weight, SKFontStyleWidth.Normal, slant))
				using (SKTypeface typeface = SKTypeface.FromFamilyName(TextFont.Family, fontStyle))
				using (SKPaint textPaint = new SKPaint() { Typeface = typeface, TextSize = textSize })
				{
					return textPaint.FontSpacing;
				}
			}
		}

		public TextFont TextFont { get; set; }

		public TextOption TextOption { get; set; }

		public bool CacheEnabled { get; set; }

		/// <summary>
		/// 创建行（支持按字母、单词自动换行）
		/// </summary>
		/// <returns></returns>
		public TextLine CreateLine()
		{
			var last = _textLines.LastOrDefault();
			if (last == null)
			{
				last = new TextLine();
				//_textLines.Add(last);
				return last;
			}

			if (TextOption.WrapMode == WrapMode.WordWrap)
			{
				//var tests = _text.Split(' ', StringSplitOptions.None);
			}
			else if (TextOption.WrapMode == WrapMode.WrapAnywhere)
			{
				var tests = _text.Split();

			}
			else // (TextOption.WrapMode == WrapMode.NoWrap)
			{

			}

			float textSize = Labels.CommonHelper.PhysicalDpiX() * TextFont.PointSize / Constants.PTS_PER_INCH;
			SKFontStyleSlant slant = TextFont.Italic ? SKFontStyleSlant.Italic : SKFontStyleSlant.Upright;

			using (SKFontStyle fontStyle = new SKFontStyle(TextFont.Weight, SKFontStyleWidth.Normal, slant))
			using (SKTypeface typeface = SKTypeface.FromFamilyName(TextFont.Family, fontStyle))
			using (SKPaint textPaint = new SKPaint() { Typeface = typeface, TextSize = textSize })
			{
				string neededString = string.Empty;

				for (int i = 1; i <= _text.Length; i++)
				{
					neededString = _text.Substring(0, i);
					SKRect rect = new SKRect();
					textPaint.MeasureText(neededString, ref rect);

					if (rect.Width > last.LineWidth)
					{
						//neededString
						break;
					}
				}
			}

			//if (_count < _textLines.Count)
			//{
			//	return _textLines[_count++];
			//}
			//else
			{
				return new TextLine();
			}
		}

		public TextLine LineAt(int index)
		{
			return _textLines[index];
		}

		public void Draw(SKCanvas painter, float x0, float y0, SKColor color)
		{
			float textSize = Labels.CommonHelper.PhysicalDpiX() * TextFont.PointSize / Constants.PTS_PER_INCH;
			SKFontStyleSlant slant = TextFont.Italic ? SKFontStyleSlant.Italic : SKFontStyleSlant.Upright;

			using (SKFontStyle fontStyle = new SKFontStyle(TextFont.Weight, SKFontStyleWidth.Normal, slant))
			using (SKTypeface typeface = SKTypeface.FromFamilyName(TextFont.Family, fontStyle))
			using (SKPaint textPaint = new SKPaint() { Typeface = typeface, TextSize = textSize })
			{
				foreach (TextLine line in _textLines)
				{
					painter.DrawText(line.Text, line.Position, textPaint);
				}
			}
		}

		public void BeginLayout()
		{
			// empty
		}

		public void EndLayout()
		{
			// empty
		}
	}
}
