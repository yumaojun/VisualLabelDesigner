using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 文本盒子
	/// </summary>
	public class ModelBoxObject : ModelShapeObject
	{
		public ModelBoxObject() { }

		public ModelBoxObject(Distance x0,
							 Distance y0,
							 Distance w,
							 Distance h,
							 bool lockAspectRatio,

							 Distance lineWidth,
							 ColorNode lineColorNode,
							 ColorNode fillColorNode,
							 SKMatrix matrix, // = SKMatrix()
							 bool shadowState, // = false

							 Distance shadowX, //  = 0f
							 Distance shadowY, //  = 0f
							 float shadowOpacity = 1.0f,

							 ColorNode shadowColorNode = null/*ColorNode()*/) :
			base(x0, y0, w, h, lockAspectRatio,
							lineWidth, lineColorNode, fillColorNode,
							matrix,
							shadowState, shadowX, shadowY, shadowOpacity, shadowColorNode)
		{

		}

		public ModelBoxObject(ModelBoxObject modelBoxObject)
		{

		}

		// Object duplication
		public override ModelObject Clone()
		{
			return new ModelBoxObject(this);
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
				SKRect rect = new SKRect(0f, 0f, _width.Pt(), _height.Pt());
				painter.DrawRect(rect, paint);
				paint.Color = lineColor;
				paint.Style = SKPaintStyle.Stroke;
				paint.StrokeWidth = _lineWidth.Pt();
				painter.DrawRect(rect, paint);
			}
		}

		/// <summary>
		/// Draw shadow of object
		/// </summary>
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
						painter.DrawRect(rect, paint);
					}
					else
					{
						/* Has FILL, but no OUTLINE. */
						var rect = new SKRect(0, 0, -_width.Pt(), _height.Pt());
						painter.DrawRect(rect, paint);
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
						var rect = new SKRect(0, 0, -_width.Pt(), _height.Pt());
						painter.DrawRect(rect, paint);
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
				float right = x + (_width + _lineWidth).Pt();
				float bottom = y + (_height + _lineWidth).Pt();
				SKRect rect = new SKRect(x, y, right, bottom);
				path.AddRect(rect);
			}
			else if (_fillColorNode.Color.Alpha > byte.MinValue && !(_lineColorNode.Color.Alpha > byte.MinValue))
			{
				SKRect rect = new SKRect(0, 0, _width.Pt(), _height.Pt());
				path.AddRect(rect);
			}
			else if (_lineColorNode.Color.Alpha > byte.MinValue)
			{
				float x = (-_lineWidth.Pt() / 2) - s * _slopPixels;
				float y = (-_lineWidth.Pt() / 2) - s * _slopPixels;
				float width = (_width + _lineWidth).Pt() + s * 2 * _slopPixels;
				float height = (_height + _lineWidth).Pt() + s * 2 * _slopPixels;
				SKRect rect = new SKRect(x, y, x + width, y + height);
				path.AddRect(rect);
				path.Close();

				x = _lineWidth.Pt() / 2 + s * _slopPixels;
				y = _lineWidth.Pt() / 2 + s * _slopPixels;
				width = (_width - _lineWidth).Pt() - s * 2 * _slopPixels;
				height = (_height - _lineWidth).Pt() - s * 2 * _slopPixels;
				SKRect other = new SKRect(x, y, x + width, y + height);
				path.AddRect(other);
			}

			return path;
		}
	}
}
