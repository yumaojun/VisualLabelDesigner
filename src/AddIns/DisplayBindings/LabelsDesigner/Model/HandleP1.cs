using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	///
	/// HandleP1 Class
	///
	public class HandleP1 : Handle
	{
		// Lifecycle Methods
		public HandleP1(ModelObject owner) : base(owner, Labels.Location.P1) { }

		public HandleP1 Clone(ModelObject newOwner)
		{
			return new HandleP1(newOwner);
		}

		// Drawing Methods
		public override void Draw(SKCanvas painter, float scale) { DrawAt(painter, scale, 0, 0, originHandleFillColor); }

		public override SKPath Path(float scale)
		{
			return PathAt(scale, 0, 0);
		}
	};

}
