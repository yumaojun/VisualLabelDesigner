using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YProgramStudio.LabelsDesigner.Backends.Merge;

namespace YProgramStudio.LabelsDesigner.Model
{
	public class ModelImageObject : ModelObject
	{
		protected static SKColor fillColor = System.Drawing.Color.FromArgb(255, 224, 224, 224).ToSKColor();
		protected static SKColor labelColor = System.Drawing.Color.FromArgb(255, 102, 102, 102).ToSKColor();
		protected static Distance pad = Distance.Pt(2f);
		protected static SKImage smDefaultImage;

		protected TextNode _filenameNode;
		protected SKImage _image;
		protected SvgRenderer _svgRenderer;
		protected byte[] _svg;

		public ModelImageObject()
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

			if (smDefaultImage == null)
			{
				string path = FileUtil.CurrentExecutingDir();
				smDefaultImage = SKImage.FromEncodedData(Path.Combine(path, "Images/checkerboard.png")); // new SKImage(":images/checkerboard.png");
			}
		}

		public ModelImageObject(Distance x0,
								Distance y0,
								Distance w,
								Distance h,
								bool lockAspectRatio,

								TextNode filenameNode,
								SKMatrix matrix, // = QMatrix()
								bool shadowState, // = false

								Distance shadowX, // = 0
								Distance shadowY, // = 0
								float shadowOpacity = 1.0f,

								ColorNode shadowColorNode = null /*ColorNode()*/ )
			: base(x0, y0, w, h, lockAspectRatio,
					   matrix,
					   shadowState, shadowX, shadowY, shadowOpacity, shadowColorNode ) 
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

			_filenameNode = filenameNode;

			_image = null;
			_svgRenderer = null;

			LoadImage();
		}

		public ModelImageObject(Distance x0,
								Distance y0,
								Distance w,
								Distance h,
								bool lockAspectRatio,

								string filename,
								SKImage image, // QImage
								SKMatrix matrix, // = QMatrix()
								bool shadowState, // = false

								Distance shadowX, // = 0
								Distance shadowY, // = 0
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

			_image = image;// TODO: 是否需要copy一个 new SKImage(image);
			_filenameNode = new TextNode(false, filename);
			_svgRenderer = null;
		}

		public ModelImageObject(Distance x0,
							   Distance y0,
							   Distance w,
							   Distance h,
							   bool lockAspectRatio,

							   string filename,
							   byte[] svg, // QByteArray
							   SKMatrix matrix, // = QMatrix()
							   bool shadowState, // = false

							   Distance shadowX, // = 0
							   Distance shadowY, // = 0
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

			_svg = svg;
			_svgRenderer = new SvgRenderer(svg);
			_filenameNode = new TextNode(false, filename);
			_image = null;
		}

		public ModelImageObject(ModelImageObject modelObject) { }

		~ModelImageObject()
		{
		}

		// Object duplication
		public override ModelObject Clone()
		{
			return new ModelImageObject(this);
		}

		protected override void DrawObject(SKCanvas painter, bool inEditor, Record record, Variables variables)
		{
			SKRect destRect = new SKRect(0, 0, 50, 50);// _width.Pt(), _height.Pt());

			if (inEditor && (_filenameNode.IsField || (_image == null && _svgRenderer == null)))
			{
				//
				// Render default place holder image
				//
				painter.Save();
				//painter->setRenderHint(QPainter::SmoothPixmapTransform, false);
				//painter->drawImage(destRect, *smDefaultImage);
				painter.DrawImage(smDefaultImage, destRect);
				painter.Restore();

				//
				// Print label on top of place holder image, if we have room
				//
				if ((_width > 6 * pad) && (_height > 4 * pad))
				{
					string labelText = TranslateHelper.Tr("No image");
					if (_filenameNode.IsField)
					{
						labelText = string.Format("{0}", _filenameNode.Data);//${%1}
					}

					// Determine font size for labelText
					//QFont font( "Sans" );
					//font.setPointSizeF(6);

					//QFontMetricsF fm(font );
					//QRectF textRect = fm.boundingRect(labelText);
					SKRect textRect = new SKRect();

					float wPts = (_width - 2 * pad).Pt();
					float hPts = (_height - 2 * pad).Pt();
					if ((wPts < textRect.Width) || (hPts < textRect.Height))
					{
						float scaleX = wPts / textRect.Width;
						float scaleY = hPts / textRect.Height;
						//font.setPointSizeF(6 * Math.Min(scaleX, scaleY));
					}

					// Render hole for text (font size may have changed above)
					//fm = QFontMetricsF(font);
					//textRect = fm.boundingRect(labelText);

					SKRect holeRect = new SKRect((_width.Pt() - textRect.Width) / 2 - pad.Pt(),
									 (_height.Pt() - textRect.Height) / 2 - pad.Pt(),
									 textRect.Width + 2 * pad.Pt(),
									 textRect.Height + 2 * pad.Pt());

					//painter->setPen(Qt::NoPen);
					//painter->setBrush(QBrush(fillColor));
					SKPaint paint = new SKPaint() { Color = fillColor, Style = SKPaintStyle.Fill, IsAntialias = true };
					painter.DrawRect(holeRect, paint);

					// Render text
					//paifnter->setFont(font);
					//painter->setPen(QPen(labelColor));
					paint.Color = labelColor;
					SKRect rect = new SKRect(0, 0, _width.Pt(), _height.Pt());
					painter.DrawText(labelText, rect.MidX, rect.MidY, paint);
				}
			}
			else if (_image != null)
			{
				painter.DrawImage(_image, destRect);
			}
			else if (_svgRenderer != null)
			{
				_svgRenderer.Render(painter, destRect);
			}
			else if (_filenameNode.IsField)
			{
				string filename = _filenameNode.Text(record, variables);
				SKImage image = null;
				SvgRenderer svgRenderer = null;
				byte[] svg = null;

				if (ReadImageFile(filename, ref image, ref svgRenderer, ref svg))
				{
					if (image != null)
					{
						painter.DrawImage(image, destRect);
					}
					else
					{
						svgRenderer.Render(painter, destRect);
					}
				}
			}
		}

		protected override void DrawShadow(SKCanvas painter, bool inEditor, Record record, Variables variables)
		{
			SKRect destRect = new SKRect(0, 0, _width.Pt(), _height.Pt());

			SKColor shadowColor = _shadowColorNode.GetColor(record, variables);
			shadowColor = shadowColor.WithAlpha((byte)_shadowOpacity);

			if (_image != null && _image.IsAlphaOnly && _image.ColorType.GetBytesPerPixel() == 32) // TODO: ->hasAlphaChannel() && (mImage->depth() == 32))
			{
				SKImage shadowImage = CreateShadowImage(_image, shadowColor);
				painter.DrawImage(shadowImage, destRect);
				shadowImage.Dispose();
			}
			else if (_image != null || _svgRenderer != null || inEditor)
			{
				SKPaint paint = new SKPaint() { Color = shadowColor, Style = SKPaintStyle.Fill, IsAntialias = true };
				painter.DrawRect(destRect, paint);
				paint.Dispose();
			}
			else
			{
				string filename = _filenameNode.Text(record, variables).Trim();
				SKImage image = null;
				SvgRenderer svgRenderer = null;
				byte[] svg = null;
				if (ReadImageFile(filename, ref image, ref svgRenderer, ref svg))
				{
					if (image != null && _image.IsAlphaOnly && _image.ColorType.GetBytesPerPixel() == 32)//image->hasAlphaChannel() && (image->depth() == 32))
					{
						SKImage shadowImage = CreateShadowImage(image, shadowColor);
						painter.DrawImage(shadowImage, destRect);
						shadowImage.Dispose();
					}
					else
					{
						SKPaint paint = new SKPaint() { Color = shadowColor, Style = SKPaintStyle.Fill, IsAntialias = true };
						painter.DrawRect(destRect, paint);
						paint.Dispose();
					}
				}
			}
		}

		protected override SKPath HoverPath(float scale)
		{
			SKPath path = new SKPath();
			path.AddRect(new SKRect(0, 0, _width.Pt(), _height.Pt()));

			return path;
		}

		private void LoadImage()
		{
			if (!_filenameNode.IsField)
			{
				string filename = _filenameNode.Data;
				if (ReadImageFile(filename, ref _image, ref _svgRenderer, ref _svg))
				{
					float aspectRatio = 0f;
					if (_svgRenderer != null)
					{
						// Adjust size based on aspect ratio of SVG image
						SKRect rect = _svgRenderer.ViewBox();
						aspectRatio = rect.Width > 0f ? rect.Height / rect.Width : 0f;
					}
					else
					{
						// Adjust size based on aspect ratio of image
						float imageW = _image.Width;
						float imageH = _image.Height;
						aspectRatio = imageW > 0f ? imageH / imageW : 0;
					}

					if (aspectRatio > 0f)
					{
						if (_height > _width * aspectRatio)
						{
							_height = _width * aspectRatio;
						}
						else
						{
							_width = _height / aspectRatio;
						}
					}
				}
			}
		}

		private bool ReadImageFile(string fileName,
								   ref SKImage image,
								   ref SvgRenderer svgRenderer,
								   ref byte[] svg)
		{
			if (Path.GetExtension(fileName).Equals("svg", StringComparison.OrdinalIgnoreCase))
			{
				svg = File.ReadAllBytes(fileName);
				svgRenderer = new SvgRenderer(svg);
			}
			else
			{
				image = SKImage.FromEncodedData(fileName);
			}

			return svgRenderer != null || image != null;
		}

		private SKImage CreateShadowImage(SKImage image, SKColor color)
		{
			byte r = color.Red;
			byte g = color.Green;
			byte b = color.Blue;
			byte a = color.Alpha;

			SKImage shadow = image.Subset(new SKRectI(0, 0, image.Width, image.Height)); // raster光栅 image; //new SKImage(image); // Subset(Rect)
			for (int iy = 0; iy < shadow.Height; iy++)
			{
				//	//auto* scanLine = (QRgb*)shadow.scanLine(iy);

				//	for (int ix = 0; ix < shadow->width(); ix++)
				//	{
				//		scanLine[ix] = qRgba(r, g, b, (a * qAlpha(scanLine[ix])) / 255);
				//	}
			}

			return shadow;
		}
	}
}
