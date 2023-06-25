using SkiaSharp;
using SkiaSharp.SimpleText;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YProgramStudio.LabelsDesigner.Backends.Merge;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 文本
	/// </summary>
	public class ModelTextObject : ModelObject
	{
		const float marginPts = 3;
		private RawText _text;
		private string _fontFamily;
		private float _fontSize;
		private SKFontStyleWeight _fontWeight;
		private bool _fontItalicFlag;
		private bool _fontUnderlineFlag;
		private ColorNode _textColorNode;
		private Alignment _textHAlign;
		private Alignment _textVAlign;
		private WrapMode _textWrapMode;
		private float _textLineSpacing;
		private bool _textAutoShrink;

		private List<TextLayout> _editorLayouts;
		private SKPath _hoverPath;

		public string Text
		{
			get => _text.ToString();
			set
			{
				if (_text != value)
				{
					_text = value;
					Update();
					OnChanged(this, null);
				}
			}
		}

		public ModelTextObject() {
			_editorLayouts = new List<TextLayout>();
		}

		public ModelTextObject(Distance x0,
							   Distance y0,
							   Distance w,
							   Distance h,
							   bool lockAspectRatio,

							   string text,
							   string fontFamily,
							   float fontSize,
							   SKFontStyleWeight fontWeight,

							   bool fontItalicFlag,

							   bool fontUnderlineFlag,
							   ColorNode textColorNode,
							   Alignment textHAlign,
							   Alignment textVAlign,
							   WrapMode textWrapMode,

							   float textLineSpacing,

							   bool textAutoShrink,

							   SKMatrix matrix, // = QMatrix()
							   bool shadowState, //  = false

							   Distance shadowX, // = 0
							   Distance shadowY, //  = 0
							   float shadowOpacity = 1.0f,

							   ColorNode shadowColorNode = null /*ColorNode()*/ )
			: base(x0, y0, w, h, lockAspectRatio,
					   matrix,
					   shadowState, shadowX, shadowY, shadowOpacity, shadowColorNode)
		{
			_outline = new Outline(this);

			_handles.Add(new HandleNorthWest(this));
			_handles.Add(new HandleNorth(this));
			_handles.Add(new HandleNorthEast(this));
			_handles.Add(new HandleEast(this));
			_handles.Add(new HandleSouthEast(this));
			_handles.Add(new HandleSouth(this));
			_handles.Add(new HandleSouthWest(this));
			_handles.Add(new HandleWest(this));

			_text = text;
			_fontFamily = fontFamily;
			_fontSize = fontSize;
			_fontWeight = fontWeight;
			_fontItalicFlag = fontItalicFlag;
			_fontUnderlineFlag = fontUnderlineFlag;
			_textColorNode = textColorNode;
			_textHAlign = textHAlign;
			_textVAlign = textVAlign;
			_textWrapMode = textWrapMode;
			_textLineSpacing = textLineSpacing;
			_textAutoShrink = textAutoShrink;

			_editorLayouts = new List<TextLayout>();
		}

		public ModelTextObject(ModelTextObject modelObject)
		{

		}

		~ModelTextObject()
		{
		}

		// Object duplication
		public override ModelObject Clone()
		{
			return new ModelTextObject(this);
		}

		/// <summary>
		/// Draw object itself
		/// </summary>
		protected override void DrawObject(SKCanvas painter, bool inEditor, Record record, Variables variables)
		{
			SKColor textColor = _textColorNode.GetColor(record, variables);

			if (inEditor)
			{
				DrawTextInEditor(painter, textColor);
			}
			else
			{
				DrawText(painter, textColor, record, variables);
			}
		}

		/// Draw text in editor from cached information
		private void DrawTextInEditor(SKCanvas painter, SKColor color)
		{
			painter.Save();

			var rect = new SKRect(0, 0, _width.Pt(), _height.Pt());
			painter.ClipRect(rect);

			SKColor mutedColor = string.IsNullOrEmpty(_text) ? color.WithAlpha((byte)(0.5 * color.Alpha)) : color;

			foreach (TextLayout layout in _editorLayouts)
			{
				layout.Draw(painter, 0f, 0f, mutedColor);
			}

			painter.Restore();
		}

		// Draw Text
		private void DrawText(SKCanvas painter, SKColor color, Backends.Merge.Record record, Variables variables)
		{
			painter.Save();

			TextFont textFont = new TextFont();
			textFont.Family = _fontFamily;
			textFont.PointSize = _fontSize;
			textFont.Weight = _fontWeight;
			textFont.Italic = _fontItalicFlag;
			textFont.Underline = _fontUnderlineFlag;

			TextOption textOption = new TextOption();
			textOption.Alignment = _textHAlign;
			textOption.WrapMode = _textWrapMode;

			TextDocument document = new TextDocument(_text);

			List<TextLayout> layouts = new List<TextLayout>();

			// Pass #1 -- do initial layouts
			float x = 0f;
			float y = 0f;
			SKRect boundingRect = SKRect.Empty;
			for (int i = 0; i < document.BlockCount; i++)
			{
				TextLayout layout = new TextLayout(document.FindBlockByNumber(i).Text);

				layout.TextFont = textFont;
				layout.TextOption = textOption;
				layout.CacheEnabled = true;

				float dy = layout.FontSpacing * _textLineSpacing;

				layout.BeginLayout();
				for (TextLine lin = layout.CreateLine(); lin.IsValid(); lin = layout.CreateLine())
				{
					lin.LineWidth = _width - 2 * marginPts;
					lin.Position = new SKPoint(x, y);
					y += dy;
				}
				layout.EndLayout();

				layouts.Add(layout);

				boundingRect = SKRect.Union(boundingRect, layout.Bounds);
			}

			float h = boundingRect.Height;

			// Pass #2 -- adjust layout positions for vertical alignment
			x = marginPts;
			switch (_textVAlign)
			{
				case Alignment.VCenter:
					y = _height / 2f - h / 2;
					break;
				case Alignment.Bottom:
					y = _height - h - marginPts;
					break;
				default:
					y = marginPts;
					break;
			}

			foreach (TextLayout layout in layouts)
			{
				float dy = layout.FontSpacing * _textLineSpacing;

				for (int j = 0; j < layout.LineCount; j++)
				{
					TextLine lin = layout.LineAt(j);
					lin.Position = new SKPoint(x, y);
					y += dy;
				}
			}

			// Draw layouts
			foreach (TextLayout layout in layouts)
			{
				layout.Draw(painter, 0f, 0f, color);
			}

			// Cleanup
			layouts.Clear();

			painter.Restore();
		}

		/// Update cached information for editor view
		private void Update()
		{
			TextFont textFont = new TextFont();
			textFont.Family = _fontFamily;
			textFont.PointSize = _fontSize;
			textFont.Weight = _fontWeight;
			textFont.Italic = _fontItalicFlag;
			textFont.Underline = _fontUnderlineFlag;

			TextOption textOption = new TextOption();
			textOption.Alignment = _textHAlign;
			textOption.WrapMode = _textWrapMode;

			string displayText = string.IsNullOrEmpty(_text) ? TranslateHelper.Tr("Text") : _text.ToString();
			TextDocument document = new TextDocument(displayText);

			_editorLayouts.Clear();

			// Pass #1 -- do initial layouts
			float x = 0;
			float y = 0;
			SKRect boundingRect = new SKRect();
			for (int i = 0; i < document.BlockCount; i++)
			{
				TextLayout layout = new TextLayout(document.FindBlockByNumber(i).Text);

				layout.TextFont = textFont;
				layout.TextOption = textOption;
				layout.CacheEnabled = true;

				float dy = layout.FontSpacing * _textLineSpacing;

				layout.BeginLayout();
				for (TextLine lin = layout.CreateLine(); lin.IsValid(); lin = layout.CreateLine())
				{
					lin.LineWidth = _width.Pt() - 2 * marginPts;
					lin.Position = new SKPoint(x, y);
					y += dy;
				}
				layout.EndLayout();

				_editorLayouts.Add(layout);

				boundingRect = SKRect.Union(boundingRect, layout.Bounds);
			}

			float h = boundingRect.Height;

			// Pass #2 -- adjust layout positions for vertical alignment and create hover path
			x = marginPts;
			switch (_textVAlign)
			{
				case Alignment.VCenter:
					y = _height.Pt() / 2 - h / 2;
					break;
				case Alignment.Bottom:
					y = _height.Pt() - h - marginPts;
					break;
				default:
					y = marginPts;
					break;
			}

			SKPath hoverPath = new SKPath(); // new empty hover path

			foreach (TextLayout layout in _editorLayouts)
			{
				float dy = layout.FontSpacing * _textLineSpacing;

				for (int j = 0; j < layout.LineCount; j++)
				{
					TextLine lin = layout.LineAt(j);
					lin.Position = new SKPoint(x, y);
					y += dy;

					hoverPath.AddRect(lin.NaturalTextRect()); // add to new hover path
				}
			}

			_hoverPath = hoverPath; // save new hover path
		}
	}
}
