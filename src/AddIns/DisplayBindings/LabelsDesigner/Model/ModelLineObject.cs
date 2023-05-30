using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YProgramStudio.LabelsDesigner.Backends.Merge;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 线
	/// </summary>
	public class ModelLineObject : ModelObject
	{
		protected Distance _lineWidth;
		protected ColorNode _lineColorNode;

		public ModelLineObject()
		{
			_outline = null;

			_handles.Add(new HandleP1(this));
			_handles.Add(new HandleP2(this));

			_lineWidth = 1.0f;
			_lineColorNode = new ColorNode();
		}

		public ModelLineObject(Distance x0,
							   Distance y0,
							   Distance w,
							   Distance h,
							   Distance lineWidth,
							   ColorNode lineColorNode,
							   SKMatrix matrix, // = QMatrix()
							   bool shadowState, //  = false

							   Distance shadowX, // = 0
							   Distance shadowY, // = 0
							   float shadowOpacity = 1.0f,

							   ColorNode shadowColorNode = null /*ColorNode()*/ )
			: base(x0, y0, w, h, false /*lockAspectRatio*/,
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

			_lineWidth = lineWidth;
			_lineColorNode = lineColorNode;
		}

		public ModelLineObject(ModelLineObject modelObject)
		{

		}

		// Object duplication
		public override ModelObject Clone()
		{
			return new ModelLineObject(this);
		}

		protected override void DrawObject(SKCanvas painter, bool inEditor, Record record, Variables variables)
		{
			SKColor lineColor = _lineColorNode.GetColor(record, variables);

			using (SKPaint paint = new SKPaint()
			{
				Color = lineColor,
				Style = SKPaintStyle.Stroke,
				StrokeWidth = _lineWidth.Pt(),
				IsAntialias = true
			})
			{
				painter.DrawLine(0, 0, _width.Pt(), _height.Pt(), paint);
			}
		}

		protected override void DrawShadow(SKCanvas painter, bool inEditor, Record record, Variables variables)
		{
			SKColor lineColor = _lineColorNode.GetColor(record, variables);
			SKColor shadowColor = _shadowColorNode.GetColor(record, variables);

			shadowColor = shadowColor.WithAlpha((byte)(_shadowOpacity * byte.MaxValue));

			if (lineColor.Alpha > byte.MinValue)
			{
				using (SKPaint paint = new SKPaint()
				{
					Color = shadowColor,
					Style = SKPaintStyle.Stroke,
					StrokeWidth = _lineWidth.Pt(),
					IsAntialias = true
				})
				{
					painter.DrawLine(0, 0, _width.Pt(), _height.Pt(), paint);
				}
			}
		}

		protected override SKPath HoverPath(float scale)
		{
			SKPath path = new SKPath();

			if (_lineColorNode.Color.Alpha > byte.MinValue)
			{
				//
				// Build a thin rectangle representing line
				//
				float rPts = _lineWidth.Pt() / 2 + _slopPixels / scale;

				float lengthPts = (float)Math.Sqrt(_width.Pt() * _width.Pt() + _height.Pt() * _height.Pt());
				float dx = _height.Pt() / lengthPts; // horizontal pitch of perpendicular line
				float dy = _width.Pt() / lengthPts; // vertical pitch of perpendicular line

				path.MoveTo(rPts * dx, -rPts * dy);
				path.LineTo(_width.Pt() + rPts * dx, _height.Pt() - rPts * dy);
				path.LineTo(_width.Pt() - rPts * dx, _height.Pt() + rPts * dy);
				path.LineTo(-rPts * dx, rPts * dy);

				path.Close();
			}

			return path;
		}
	}
}
