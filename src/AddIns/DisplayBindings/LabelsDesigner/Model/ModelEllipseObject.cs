using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	public class ModelEllipseObject : ModelShapeObject
	{
		public ModelEllipseObject() { }

		public ModelEllipseObject(Distance x0,
								  Distance y0,
								  Distance w,
								  Distance h,
								  bool lockAspectRatio,

								  Distance lineWidth,
								  ColorNode lineColorNode,
								  ColorNode fillColorNode,
								  SKMatrix matrix, // = QMatrix()
								  bool shadowState, // = false

								  Distance shadowX, //= 0
								  Distance shadowY, //= 0
								  float shadowOpacity = 1.0f,

								  ColorNode shadowColorNode = null /*ColorNode()*/ )
			: base(x0, y0, w, h, lockAspectRatio,
							lineWidth, lineColorNode, fillColorNode,
							matrix,
							shadowState, shadowX, shadowY, shadowOpacity, shadowColorNode)
		{
			// nothing
		}

		public ModelEllipseObject(ModelEllipseObject modelEllipseObject)
		{
		}

		// Object duplication
		public override ModelObject Clone()
		{
			return new ModelEllipseObject(this);
		}

		/// <summary>
		/// Draw object itself
		/// </summary>
		protected override void DrawObject(SKCanvas painter, bool inEditor, Backends.Merge.Record record, Variables variables)
		{
			SKColor lineColor = _lineColorNode.GetColor(record, variables);
			SKColor fillColor = _fillColorNode.GetColor(record, variables);

			using (SKPaint paint = new SKPaint()
			{
				Color = fillColor,
				Style = SKPaintStyle.Fill,
				IsAntialias = true
			})
			{
				SKRect rect = new SKRect(0, 0, _width.Pt(), _height.Pt());
				painter.DrawOval(rect, paint);
				paint.Color = lineColor;
				paint.Style = SKPaintStyle.Stroke;
				paint.StrokeWidth = _lineWidth.Pt();
				painter.DrawOval(rect, paint);
			}
		}

		///<summary>
		/// Draw shadow of object
		///</summary>
		protected override void DrawShadow(SKCanvas painter, bool inEditor, Backends.Merge.Record record, Variables variables)
		{
			SKColor lineColor = _lineColorNode.GetColor(record, variables);
			SKColor fillColor = _fillColorNode.GetColor(record, variables);
			SKColor shadowColor = _shadowColorNode.GetColor(record, variables);

			shadowColor = shadowColor.WithAlpha((byte)(_shadowOpacity * byte.MaxValue));

			if (fillColor.Alpha > byte.MinValue)
			{
				using (SKPaint paint = new SKPaint()
				{
					Color = shadowColor,
					Style = SKPaintStyle.Fill,
					IsAntialias = true
				})
				{
					if (lineColor.Alpha > byte.MinValue)
					{
						/* Has FILL and OUTLINE: adjust size to account for line width. */
						var rect = new SKRect(-_lineWidth.Pt() / 2, -_lineWidth.Pt() / 2, (_width + _lineWidth).Pt(), (_height + _lineWidth).Pt());
						painter.DrawOval(rect, paint);
					}
					else
					{
						/* Has FILL, but no OUTLINE. */
						var rect = new SKRect(0, 0, _width.Pt(), _height.Pt());
						painter.DrawOval(rect, paint);
					}
				}
			}
			else
			{
				if (lineColor.Alpha > byte.MinValue)
				{
					/* Has only OUTLINE. */
					using (SKPaint paint = new SKPaint()
					{
						Color = shadowColor,
						Style = SKPaintStyle.Stroke,
						StrokeWidth = _lineWidth.Pt(),
						IsAntialias = true
					})
					{
						var rect = new SKRect(0, 0, _width.Pt(), _height.Pt());
						painter.DrawOval(rect, paint);
					}
				}
			}
		}

		/// <summary>
		/// Path to test for hover condition
		/// </summary>
		protected override SKPath HoverPath(float scale)
		{
			float s = 1 / scale;

			SKPath path = new SKPath();

			if (_fillColorNode.Color.Alpha > byte.MinValue && _lineColorNode.Color.Alpha > byte.MinValue)
			{
				float x = -_lineWidth.Pt() / 2;
				float y = -_lineWidth.Pt() / 2;
				float width = (_width + _lineWidth).Pt();
				float height = (_height + _lineWidth).Pt();
				SKRect rect = new SKRect(x, y, x + width, y + height);
				path.AddOval(rect);
			}
			else if (_fillColorNode.Color.Alpha > byte.MinValue && !(_lineColorNode.Color.Alpha > byte.MinValue))
			{
				SKRect rect = new SKRect(0, 0, _width.Pt(), _height.Pt());
				path.AddOval(rect);
			}
			else if (_lineColorNode.Color.Alpha > byte.MinValue)
			{
				float x = (-_lineWidth.Pt() / 2) - s * _slopPixels;
				float y = (-_lineWidth.Pt() / 2) - s * _slopPixels;
				float width = (_width + _lineWidth).Pt() + s * 2 * _slopPixels;
				float height = (_height + _lineWidth).Pt() + s * 2 * _slopPixels;
				SKRect rect = new SKRect(x, y, x + width, y + height);
				path.AddOval(rect);
				path.Close();

				x = _lineWidth.Pt() / 2 + s * _slopPixels;
				y = _lineWidth.Pt() / 2 + s * _slopPixels;
				width = (_width - _lineWidth).Pt() - s * 2 * _slopPixels;
				height = (_height - _lineWidth).Pt() - s * 2 * _slopPixels;
				SKRect other = new SKRect(x, y, x + width, y + height);
				path.AddOval(other);
			}

			return path;
		}

	}
}
