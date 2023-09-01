using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YProgramStudio.LabelsDesigner.Backends.Barcode;
using YProgramStudio.LabelsDesigner.Backends.Merge;
using YProgramStudio.LabelsDesigner.Labels;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 条码
	/// </summary>
	public class ModelBarcodeObject : ModelObject
	{
		#region Const Data

		public static readonly SKColor fillColor = System.Drawing.Color.FromArgb(255, 224, 224, 224).ToSKColor();
		public static readonly Distance pad = Distance.Pt(4);
		public static readonly Distance minWidth = Distance.Pt(18);
		public static readonly Distance minHeight = Distance.Pt(18);

		#endregion

		#region Private Data

		private BarcodeStyle _bcStyle;
		private bool _bcTextFlag;
		private bool _bcChecksumFlag;
		private int _bcFormatDigits;
		private RawText _bcData;
		private ColorNode _bcColorNode;

		private Barcodes.Barcode _editorBarcode;
		private Barcodes.Barcode _editorDefaultBarcode;

		private SKPath _hoverPath;

		#endregion

		public ModelBarcodeObject()
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

			_bcStyle = Backends.Barcode.Backends.DefaultStyle();
			_bcTextFlag = _bcStyle.CanText;
			_bcChecksumFlag = _bcStyle.CanChecksum;
			_bcFormatDigits = _bcStyle.PreferedN;
			_bcData = string.Empty;
			_bcColorNode = new ColorNode(SKColors.Black);

			_editorBarcode = null;
			_editorDefaultBarcode = null;

			// Initialize cached editor layouts
			Update();
		}

		public ModelBarcodeObject(Distance x0,
								 Distance y0,
								 Distance w,
								 Distance h,
								 bool lockAspectRatio,

								 BarcodeStyle bcStyle,
								 bool bcTextFlag,

								 bool bcChecksumFlag,
								 string bcData,
								 ColorNode bcColorNode,
								 SKMatrix matrix /*= QMatrix()*/ )
			: base(x0, y0, w, h, lockAspectRatio, matrix)
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

			_bcStyle = bcStyle;
			_bcTextFlag = bcTextFlag;
			_bcChecksumFlag = bcChecksumFlag;
			_bcFormatDigits = _bcStyle.PreferedN;
			_bcData = bcData;
			_bcColorNode = bcColorNode;

			_editorBarcode = null;
			_editorDefaultBarcode = null;

			// Initialize cached editor layouts
			Update();
		}

		public ModelBarcodeObject(ModelBarcodeObject modelObject)
		{
		}

		~ModelBarcodeObject()
		{
		}

		// Object duplication
		public override ModelObject Clone()
		{
			return new ModelBarcodeObject(this);
		}

		/// <summary>
		/// BarcodeStyle
		/// </summary>
		public BarcodeStyle BcStyle
		{
			get => _bcStyle;
			set
			{
				if (_bcStyle != value)
				{
					_bcStyle = value;
					Update();
					OnChanged(this, null);
				}
			}
		}

		public bool BcTextFlag
		{
			get => _bcTextFlag;
			set
			{
				_bcTextFlag = value;
				Update();
				OnChanged(this, null);
			}
		}

		public bool BcChecksumFlag
		{
			get => _bcChecksumFlag;
			set
			{
				_bcChecksumFlag = value;
				Update();
				OnChanged(this, null);
			}
		}

		public RawText BcData
		{
			get => _bcData;
			set
			{
				_bcData = value;
				Update();
				OnChanged(this, null);
			}
		}

		public ColorNode BcColorNode
		{
			get => _bcColorNode;
			set
			{
				_bcColorNode = value;
				Update();
				OnChanged(this, null);
			}
		}

		protected override void DrawObject(SKCanvas painter, bool inEditor, Record record, Variables variables)
		{
			SKColor bcColor = _bcColorNode.GetColor(record, variables);

			if (inEditor)
			{
				DrawBcInEditor(painter, bcColor);
			}
			else
			{
				DrawBc(painter, bcColor, record, variables);
			}
		}

		protected override void DrawShadow(SKCanvas painter, bool inEditor, Record record, Variables variables)
		{
			// Barcodes don't support shadows.
		}

		protected override SKPath HoverPath(float scale)
		{
			return _hoverPath;
		}

		/// <summary>
		/// Size updated
		/// </summary>
		protected override void SizeUpdated()
		{
			Update();
		}

		#region Private methods

		/// <summary>
		/// Update cached information for editor view
		/// </summary>
		private void Update()
		{
			// Build barcode from data
			_editorBarcode = Barcodes.Factory.CreateBarcode(_bcStyle.FullId); // ToStdString()
			if (_editorBarcode == null)
			{
				//qWarning() << "Invalid barcode style" << _bcStyle.fullId() << "using \"code39\".";
				_bcStyle = Backends.Barcode.Backends.DefaultStyle();
				_editorBarcode = Barcodes.Factory.CreateBarcode(_bcStyle.Id); // ().toStdString()
			}
			_editorBarcode.Checksum = _bcChecksumFlag;
			_editorBarcode.ShowText = _bcTextFlag;

			_editorBarcode.Build(_bcData.ToString(), _width.Pt(), _height.Pt());//.ToStdString()

			// Build a place holder barcode to display in editor, if cannot display actual barcode
			_editorDefaultBarcode = Barcodes.Factory.CreateBarcode(_bcStyle.FullId); // ().toStdString()
			if (_editorDefaultBarcode == null)
			{
				// Warning() << "Invalid barcode style" << _bcStyle.fullId() << "using \"code39\".";
				_bcStyle = Backends.Barcode.Backends.DefaultStyle();
				_editorDefaultBarcode = Barcodes.Factory.CreateBarcode(_bcStyle.Id); // ().toStdString()
			}
			_editorDefaultBarcode.Checksum = _bcChecksumFlag;
			_editorDefaultBarcode.ShowText = _bcTextFlag;

			_editorDefaultBarcode.Build(_bcStyle.DefaultDigits, _width.Pt(), _height.Pt()); //.toStdString()

			// Adjust size
			if (_editorBarcode.IsDataValid)
			{
				_width = Distance.Pt(_editorBarcode.Width);
				_height = Distance.Pt(_editorBarcode.Height);
			}
			else
			{
				_width = Distance.Pt(_editorDefaultBarcode.Width);
				_height = Distance.Pt(_editorDefaultBarcode.Height);
			}

			SKPath path = new SKPath();
			path.AddRect(new SKRect(0, 0, _width.Pt(), _height.Pt()));
			_hoverPath = path;
		}

		// Draw barcode in editor from cached information
		private void DrawBcInEditor(SKCanvas painter, SKColor color)
		{
			if (_bcData.IsEmpty())
			{
				DrawPlaceHolder(painter, color, TranslateHelper.Tr("No barcode data"));
			}
			else if (_bcData.HasPlaceHolders())
			{
				DrawPlaceHolder(painter, color, _bcData.ToString());
			}
			else if (_editorBarcode.IsDataValid)
			{
				Barcodes.Renderer renderer = new Barcodes.Renderer(painter, color);
				_editorBarcode.Render(renderer);
			}
			else
			{
				DrawPlaceHolder(painter, color, TranslateHelper.Tr("Invalid barcode data"));
			}
		}

		private void DrawBc(SKCanvas painter, SKColor color, Record record, Variables variables)
		{
			Barcodes.Barcode bc = Barcodes.Factory.CreateBarcode(_bcStyle.FullId); // .ToStdString()
			bc.Checksum = _bcChecksumFlag;
			bc.ShowText = _bcTextFlag;

			bc.Build(_bcData.Expand(record, variables), _width.Pt(), _height.Pt()); // .ToStdString()

			Barcodes.Renderer renderer = new Barcodes.Renderer(painter, color);
			bc.Render(renderer);
		}

		private void DrawPlaceHolder(SKCanvas painter, SKColor color, string text)
		{
			string shortText = text.Length > 32 ? text.Substring(0, 32) : text; // Don't let the text get out of hand

			// Render box
			using (SKPaint paint = new SKPaint() { Color = fillColor, Style = SKPaintStyle.Fill })
			{
				painter.DrawRect(new SKRect(0, 0, _width.Pt(), _height.Pt()), paint);
			}

			// Render default barcode
			Barcodes.Renderer renderer = new Barcodes.Renderer(painter, color);
			_editorDefaultBarcode.Render(renderer);

			//
			// Determine font size for text
			//
			//QFont font( "Sans" );
			//font.setPointSizeF(6);

			//QFontMetricsF fm(font );
			//QRectF textRect = fm.boundingRect(shortText);
			SKTypeface typeface = SKTypeface.FromFamilyName("Sans");
			//SKIntToScalar();
			SKPaint textPaint = new SKPaint() { Typeface = typeface, TextSize = 6 };
			SKRect textRect = new SKRect();
			textPaint.MeasureText(shortText, ref textRect);

			float wPts = (_width - 2 * pad).Pt();
			float hPts = (_height - 2 * pad).Pt();
			if ((wPts < textRect.Width) || (hPts < textRect.Height))
			{
				float scaleX = wPts / textRect.Width;
				float scaleY = hPts / textRect.Height;
				textPaint.TextSize = (6 * Math.Min(scaleX, scaleY));
			}

			//
			// Render hole for text (font size may have changed above)
			//
			//fm = QFontMetricsF(font);
			//textRect = fm.boundingRect(shortText);
			textPaint.MeasureText(shortText, ref textRect);

			float x1 = (_width.Pt() - textRect.Width) / 2 - pad.Pt();
			float y1 = (_height.Pt() - textRect.Height) / 2 - pad.Pt();
			float left1 = (_width.Pt() - textRect.Width) / 2 - pad.Pt() + textRect.Width + 2 * pad.Pt();
			float bottom1 = (_height.Pt() - textRect.Height) / 2 - pad.Pt() + textRect.Height + 2 * pad.Pt();
			SKRect holeRect = new SKRect(x1, y1, left1, bottom1);

			using (SKPaint paint = new SKPaint() { Color = fillColor, Style = SKPaintStyle.Fill })
			{
				painter.DrawRect(holeRect, paint);
			}

			//
			// Render text
			//
			//painter->setFont(font);
			//painter->setPen(QPen(color));
			SKRect rect = new SKRect(0, 0, _width.Pt(), _height.Pt());
			DrawTextInRectCenter(painter, shortText, rect, color);
		}

		// TODO：textsize是像素，QT的font.setPointSizeF(6);是什么，然后字形打印是什么位置
		private void DrawTextInRectCenter(SKCanvas painter, string text, SKRect rect, SKColor color)
		{
			using (SKTypeface typeface = SKTypeface.FromFamilyName("Sans"))
			using (SKPaint paint = new SKPaint() { Color = color, Typeface = typeface, TextSize = 6, IsAntialias = true })
			{
				SKRect textRect = new SKRect();
				paint.MeasureText(text, ref textRect);
				painter.DrawText(text, rect.MidX - textRect.MidX, rect.MidY + textRect.Height - pad.Pt(), paint);
			}
		}

		#endregion
	}
}
