using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiaSharp.SimpleText
{
	/// <summary>
	/// Text Layout
	/// </summary>
	public class STTextLayout
	{
		private string _text;
		private STFont _font;
		private STTextOption _textOption;
		private bool _cacheEnabled;
		private List<STTextLine> _textLines;

		public string Text { get => _text; }

		public STFont Font { get => _font; set => _font = value; }

		public STTextOption TextOption { get => _textOption; set => _textOption = value; }

		public bool CacheEnabled { get => _cacheEnabled; set => _cacheEnabled = value; }

		public SKRect Bounds
		{
			get
			{
				SKRect rect = new SKRect();
				paint.MeasureText(_text, ref rect);
				return rect;
			}
		}

		public int LineCount { get; }

		public STTextLayout()
		{
			_text = string.Empty;
			_textLines = new List<STTextLine>();
		}

		public STTextLayout(string text)
		{
			_text = text;
			_textLines = new List<STTextLine>();
			_textLines.Add(new STTextLine(text));
		}

		public STTextLayout(string text, STFont font, STTextOption textOption, bool cacheEnabled = false)
		{
			_text = text;
			_font = font;
			_textOption = textOption;
			_cacheEnabled = cacheEnabled;
			_textLines = new List<STTextLine>();
		}

		public STTextLine LineAt(int index)
		{
			return _textLines[index];
		}

		private int _count = 0;
		public STTextLine CreateLine()
		{
			if (_count < _textLines.Count)
			{
				return _textLines[_count++];
			}
			else
			{
				return new STTextLine();
			}
		}

		public void BeginLayout()
		{
		}

		public void EndLayout()
		{
		}

		SKPaint paint = new SKPaint()
		{
			Color = new SKColor(1, 1, 255),//SKColors
			Style = SKPaintStyle.Fill,
			TextSize = 26,
			FakeBoldText = true
		};

		public void Draw(SKCanvas painter, SKPoint point)
		{
			//var fontManager = SKFontManager.Default;
			//var emojiTypeface = fontManager..MatchCharacter(_text);

			var index = SKFontManager.Default.FontFamilies.ToList().IndexOf("宋体");
			var songtiTypeface = SKFontManager.Default.GetFontStyles(index).CreateTypeface(0);
			paint.Typeface = songtiTypeface;

			//using (SKPaint paint1 = new SKPaint() { })
			{//_font.ToSKFont(),
				painter.DrawText(_text, point.X, point.Y + 40f,  paint);
			}
		}
	}
}
