using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	///
	/// HandleNorthWest Class
	///
	public class HandleNorthWest : Handle
	{
		// Lifecycle Methods
		public HandleNorthWest(ModelObject owner) : base(owner, Labels.Location.NW) { }

		public HandleNorthWest Clone(ModelObject newOwner)
		{
			return new HandleNorthWest(newOwner);
		}

		// Drawing Methods
		public override void Draw(SKCanvas painter, float scale) { DrawAt(painter, scale, 0, 0, originHandleFillColor); }

		public override SKPath Path(float scale)
		{
			return PathAt(scale, 0, 0);
		}
	};
}
