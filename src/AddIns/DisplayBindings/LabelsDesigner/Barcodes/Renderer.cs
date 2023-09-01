using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YProgramStudio.LabelsDesigner.Labels;
using YProgramStudio.LabelsDesigner.Model;

namespace YProgramStudio.LabelsDesigner.Barcodes
{
	/// <summary>
	/// 呈现条码
	/// </summary>
	public class Renderer
	{
		private const float FONT_SCALE = 0.75f;
		private RendererData data;

		public Renderer()
		{
			data = new RendererData();
			data.Painter = null;
		}

		public Renderer(SKCanvas painter, SKColor color)
		{
			data = new RendererData();
			data.Painter = painter;
			data.Color = color;
		}

		public void Render(float w, float h, List<DrawingPrimitive> primitives)
		{
			DrawBegin(w, h);

			foreach (DrawingPrimitive primitive in primitives)
			{
				if (primitive is DrawingPrimitiveLine line)
				{
					DrawLine(line.X, line.Y, line.Width, line.Height);
				}
				else if (primitive is DrawingPrimitiveBox box)
				{
					DrawBox(box.X, box.Y, box.Width, box.Height);
				}
				else if (primitive is DrawingPrimitiveText text)
				{
					DrawText(text.X, text.Y, text.Size, text.Text);
				}
				else if (primitive is DrawingPrimitiveRing ring)
				{
					DrawRing(ring.X, ring.Y, ring.Radius, ring.Width);
				}
				else if (primitive is DrawingPrimitiveHexagon hex)
				{
					DrawHexagon(hex.X, hex.Y, hex.Height);
				}
			}

			DrawEnd();
		}

		protected void DrawBegin(float w, float h)
		{
			if (data.Painter != null)
			{
				data.Painter.Save();
				//data.Color = data.Painter->pen().color(); // Get current pen color
			}
		}

		protected void DrawEnd()
		{
			if (data.Painter != null)
			{
				data.Painter.Restore();
			}
		}

		protected virtual void DrawLine(float x, float y, float w, float h)
		{
			if (data.Painter != null)
			{
				float x1 = x + w / 2; // Offset line origin by 1/2 line width.

				using (SKPaint paint = new SKPaint()
				{
					Color = data.Color,
					Style = SKPaintStyle.Stroke,
					StrokeWidth = w,
					IsAntialias = true
				})
				{
					data.Painter.DrawLine(new SKPoint(x1, y), new SKPoint(x1, y + h), paint);
				}
			}
		}

		protected virtual void DrawBox(float x, float y, float w, float h)
		{
			if (data.Painter != null)
			{
				using (SKPaint paint = new SKPaint()
				{
					Color = data.Color,
					Style = SKPaintStyle.Fill,
					IsAntialias = true
				})
				{
					data.Painter.DrawRect(new SKRect(x, y, x + w, y + h), paint); // 开启了IsAntialias方框四周会出现模糊的细线
				}
			}
		}

		protected virtual void DrawText(float x, float y, float size, string text)
		{
			//if (d->painter)
			//{
			//	d->painter->setPen(QPen(d->color));

			//	QFont font;
			//	font.setStyleHint(QFont::Monospace);
			//	font.setFamily("monospace");
			//	font.setPointSizeF(FONT_SCALE * size);

			//	QFontMetricsF fm(font );
			//	double xCorner = x - fm.width(QString::fromStdString(text)) / 2.0;
			//	double yCorner = y - fm.ascent();

			//	QTextLayout layout(QString::fromStdString(text), font );
			//	layout.beginLayout();
			//	layout.createLine();
			//	layout.endLayout();
			//	layout.draw(d->painter, QPointF(xCorner, yCorner));
			//}

			float fontSize = FONT_SCALE * size * 96f / Constants.PTS_PER_INCH; // TODO: 条码输出字体的大小问题，pt -> pixels
			using (SKTypeface face = SKTypeface.FromFamilyName("Courier New", /* todo: 提取为配置，等宽字体？monospace*/
				SKFontStyleWeight.Thin, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright))
			using (SKPaint paint = new SKPaint()
			{
				Color = data.Color,
				IsAntialias = true,
				Style = SKPaintStyle.Fill,
				TextSize = fontSize,
				Typeface = face
			})
			{
				SKRect textRect = new SKRect();
				paint.MeasureText(text, ref textRect);
				float xCorner = x - textRect.Width / 2;
				float yCorner = y; // - textRect.Height - paint.FontMetrics.Ascent; // TODO: 条码输出字体的基线问题
				data.Painter.DrawText(text, xCorner, yCorner, paint);
			}
		}

		protected virtual void DrawRing(float x, float y, float r, float w)
		{
			if (data.Painter != null)
			{
				using (SKPaint paint = new SKPaint() { Color = data.Color, Style = SKPaintStyle.Stroke, StrokeWidth = w })
				{
					data.Painter.DrawOval(x, y, r, r, paint);
				}
			}
		}

		// 六角形\六边形
		protected virtual void DrawHexagon(double x, double y, double h)
		{
			if (data.Painter != null)
			{
				//d->painter->setPen(QPen(Qt::NoPen));
				//d->painter->setBrush(QBrush(d->color));

				//QPolygonF hexagon;
				//hexagon << QPointF(x, y)
				//		<< QPointF(x + 0.433 * h, y + 0.25 * h)
				//		<< QPointF(x + 0.433 * h, y + 0.75 * h)
				//		<< QPointF(x, y + h)
				//		<< QPointF(x - 0.433 * h, y + 0.75 * h)
				//		<< QPointF(x - 0.433 * h, y + 0.25 * h);

				//data.Painter.DrawRegion();// drawPolygon(hexagon);
			}
		}
	}
}
