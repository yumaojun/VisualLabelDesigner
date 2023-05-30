using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YProgramStudio.LabelsDesigner.Backends.Barcode;
using YProgramStudio.LabelsDesigner.Backends.Merge;

namespace YProgramStudio.LabelsDesigner.Model
{
	public class ModelBarcodeObject : ModelObject
	{
		public ModelBarcodeObject() { }

		public ModelBarcodeObject(Distance x0,
								 Distance y0,
								 Distance w,
								 Distance h,
								 bool lockAspectRatio,

								 Style bcStyle,
								 bool bcTextFlag,

								 bool bcChecksumFlag,
								 string bcData,
								 ColorNode bcColorNode,
								 SKMatrix matrix /*= QMatrix()*/ )
		{
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
