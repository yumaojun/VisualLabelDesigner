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

		//private List<TextLayout> mEditorLayouts;
		private SKPath _hoverPath;

		public string Text
		{
			get => _text.ToString();
			set
			{
				if (_text != value)
				{
					_text = value;
					//update();
					OnChanged(this, null);
				}
			}
		}

		public ModelTextObject() { }

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

			if (false) // inEditor
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

			if (string.IsNullOrEmpty(_text))
			{
				SKColor mutedColor = color;
				mutedColor = mutedColor.WithAlpha((byte)(0.5 * color.Alpha));
				//painter->setPen(QPen(mutedColor ) );
			}
			else
			{
				//painter->setPen(QPen(color ) );
			}

			//foreach (QTextLayout* layout, mEditorLayouts )
			//			{
			//	layout->draw(painter, QPointF(0, 0));
			//}

			using (SKPaint paint = new SKPaint()
			{
				Color = color
			})
			{
				painter.DrawText(_text, _x0.Pt(), _y0.Pt(), paint);
			}

			painter.Restore();
		}

		// Draw Text
		private void DrawText(SKCanvas painter, SKColor color, Backends.Merge.Record record, Variables variables)
		{
			painter.Save();

			var rect = new SKRect(0, 0, _width, _height);
			//painter.ClipRect(rect );
			//var paint = new SKPaint() { Color = color, Style = SKPaintStyle.Stroke, StrokeWidth = 1 };
			//painter.DrawRect(rect, paint); // 外框

			STFontWeight mFontWeight = STFontWeight.Normal; // 字体粗细
			STAlignment mTextHAlign = STAlignment.AlignRight; // 水平居中
			STAlignment mTextVAlign = STAlignment.AlignVCenter; // 垂直居中
			STWrapMode mTextWrapMode = STWrapMode.NoWrap; // 自动换行：Word, AnyWhere, NoWrap
			float mTextLineSpacing = 1; // 行间距
			float marginPts = 3;

			STFont font = new STFont();
			font.Family = _fontFamily;
			font.PointSize = _fontSize;
			font.Weight = mFontWeight;
			font.Italic = _fontItalicFlag;
			font.Underline = _fontUnderlineFlag;

			STTextOption textOption = new STTextOption();
			textOption.Alignment = mTextHAlign;
			textOption.WrapMode = mTextWrapMode;

			STFontMetrics fontMetrics = new STFontMetrics(font);
			float dy = fontMetrics.LineSpacing * mTextLineSpacing;

			STTextDocument document = new STTextDocument(_text);

			List<STTextLayout> layouts = new List<STTextLayout>();

			// Pass #1 -- do initial layouts
			float x = 0f;
			float y = 0f;
			SKRect boundingRect = SKRect.Empty;
			for (int i = 0; i < document.BlockCount; i++)
			{
				STTextLayout layout = new STTextLayout(document.FindBlockByNumber(i).Text);

				layout.Font = font;
				layout.TextOption = textOption;
				layout.CacheEnabled = true;

				layout.BeginLayout();
				for (STTextLine l = layout.CreateLine(); l.IsValid(); l = layout.CreateLine())
				{
					l.LineWidth = _width - 2 * marginPts;
					l.Position = new SKPoint(x, y);
					y += dy;
				}
				layout.EndLayout();

				layouts.Add(layout);

				//boundingRect = layout->boundingRect().united( boundingRect );
				var temp = boundingRect;
				boundingRect = layout.Bounds;
				boundingRect.Union(temp);
			}
			var h = boundingRect.Height;

			// Pass #2 -- adjust layout positions for vertical alignment
			x = marginPts;
			switch (mTextVAlign)
			{
				case STAlignment.AlignVCenter:
					y = _height / 2f - h / 2;
					break;
				case STAlignment.AlignBottom:
					y = _height - h - marginPts;
					break;
				default:
					y = marginPts;
					break;
			}

			foreach (STTextLayout layout in layouts)
			{
				for (int j = 0; j < layout.LineCount; j++)
				{
					STTextLine l = layout.LineAt(j);
					l.Position = new SKPoint(x, y);
					y += dy;
				}
			}

			// Draw layouts
			//painter->setPen(QPen(color));
			foreach (STTextLayout layout in layouts)
			{
				layout.Draw(painter, new SKPoint(0, 0));
			}

			// Cleanup
			layouts.Clear();

			painter.Restore();
		}

		///
		/// Update cached information for editor view
		///
		private void Update()
		{
			//SKFont font = new SKFont();
			//font.setFamily(_fontFamily);
			//font.setPointSizeF(_fontSize);
			//font.setWeight(_fontWeight);
			//font.setItalic(_fontItalicFlag);
			//font.setUnderline(_fontUnderlineFlag);

			//QTextOption textOption;
			//textOption.setAlignment(mTextHAlign);
			//textOption.setWrapMode(mTextWrapMode);

			//QFontMetricsF fontMetrics(font );
			//double dy = fontMetrics.lineSpacing() * mTextLineSpacing;

			//QString displayText = mText.isEmpty() ? tr("Text") : mText.toString();
			//QTextDocument document(displayText );

			//qDeleteAll(mEditorLayouts);
			//mEditorLayouts.clear();

			//// Pass #1 -- do initial layouts
			//double x = 0;
			//double y = 0;
			//QRectF boundingRect;
			//for (int i = 0; i < document.blockCount(); i++)
			//{
			//	QTextLayout* layout = new QTextLayout(document.findBlockByNumber(i).text());

			//	layout->setFont(font);
			//	layout->setTextOption(textOption);
			//	layout->setCacheEnabled(true);

			//	layout->beginLayout();
			//	for (QTextLine l = layout->createLine(); l.isValid(); l = layout->createLine())
			//	{
			//		l.setLineWidth(mW.pt() - 2 * marginPts);
			//		l.setPosition(QPointF(x, y));
			//		y += dy;
			//	}
			//	layout->endLayout();

			//	mEditorLayouts.append(layout);

			//	boundingRect = layout->boundingRect().united(boundingRect);
			//}
			//double h = boundingRect.height();


			//// Pass #2 -- adjust layout positions for vertical alignment and create hover path
			//x = marginPts;
			//switch (mTextVAlign)
			//{
			//	case Qt::AlignVCenter:
			//		y = mH.pt() / 2 - h / 2;
			//		break;
			//	case Qt::AlignBottom:
			//		y = mH.pt() - h - marginPts;
			//		break;
			//	default:
			//		y = marginPts;
			//		break;
			//}
			//QPainterPath hoverPath; // new empty hover path
			//foreach (QTextLayout* layout, mEditorLayouts )
			//{
			//	for (int j = 0; j < layout->lineCount(); j++)
			//	{
			//		QTextLine l = layout->lineAt(j);
			//		l.setPosition(QPointF(x, y));
			//		y += dy;

			//		hoverPath.addRect(l.naturalTextRect()); // add to new hover path
			//	}
			//}

			//mHoverPath = hoverPath; // save new hover path
		}
	}
}
