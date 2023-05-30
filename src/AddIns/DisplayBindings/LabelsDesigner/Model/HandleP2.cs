using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{

	///
	/// HandleP2 Class
	///
	public class HandleP2 : Handle
	{
		// Lifecycle Methods
		public HandleP2(ModelObject owner) : base(owner, Labels.Location.P2) { }

		// Duplication
		public HandleP2 Clone(ModelObject newOwner)
		{
			return new HandleP2(newOwner);
		}

		// Drawing Methods
		public override void Draw(SKCanvas painter, float scale) { DrawAt(painter, scale, _owner.Width, _owner.Height, handleFillColor); }

		public override SKPath Path(float scale)
		{
			return PathAt(scale, _owner.Width, _owner.Height);
		}
	};
}
