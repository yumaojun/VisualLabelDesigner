using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YProgramStudio.LabelsDesigner.Backends.Merge;

namespace YProgramStudio.LabelsDesigner.Model
{
	public class ModelImageObject : ModelObject
	{
		public ModelImageObject() { }

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
		{ }

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
		{ }

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
		{ }

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
			throw new NotImplementedException();
		}

		protected override void DrawShadow(SKCanvas painter, bool inEditor, Record record, Variables variables)
		{
			throw new NotImplementedException();
		}

		protected override SKPath HoverPath(float scale)
		{
			throw new NotImplementedException();
		}
	}
}
