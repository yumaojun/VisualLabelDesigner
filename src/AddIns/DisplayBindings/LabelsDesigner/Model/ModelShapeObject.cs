using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YProgramStudio.LabelsDesigner.Backends.Merge;

namespace YProgramStudio.LabelsDesigner.Model
{
	public class ModelShapeObject : ModelObject
	{
		protected Distance _lineWidth;
		protected ColorNode _lineColorNode;
		protected ColorNode _fillColorNode;

		public ModelShapeObject() {
			_outline = new Outline(this);

			_handles.Add(new HandleNorthWest(this));
			_handles.Add(new HandleNorth(this));
			_handles.Add(new HandleNorthEast(this));
			_handles.Add(new HandleEast(this));
			_handles.Add(new HandleSouthEast(this));
			_handles.Add(new HandleSouth(this));
			_handles.Add(new HandleSouthWest(this));
			_handles.Add(new HandleWest(this));

			_lineWidth = 1.0f;
			_lineColorNode = new ColorNode(System.Drawing.Color.FromArgb(0, 0, 0).ToSKColor());
			_fillColorNode = new ColorNode(System.Drawing.Color.FromArgb(0, 255, 0).ToSKColor());
		}

		public ModelShapeObject(Distance x0,
								Distance y0,
								Distance w,
								Distance h,
								bool lockAspectRatio,

								Distance lineWidth,
								ColorNode lineColorNode,
								ColorNode fillColorNode,
								SKMatrix matrix,
								bool shadowState,

								Distance shadowX,
								Distance shadowY,
								float shadowOpacity,

								ColorNode shadowColorNode)
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

			_lineWidth = lineWidth;
			_lineColorNode = lineColorNode;
			_fillColorNode = fillColorNode;
		}
	}
}
