using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	///
	/// HandleNorthEast Class
	///
	public class HandleNorthEast : Handle
	{
		// Lifecycle Methods
		public HandleNorthEast(ModelObject owner) : base(owner, Labels.Location.NE) { }

		public HandleNorthEast Clone(ModelObject newOwner)
		{
			return new HandleNorthEast(newOwner);
		}

		// Drawing Methods
		public override void Draw(SKCanvas painter, float scale)
		{
			DrawAt(painter, scale, _owner.Width, 0, handleFillColor);
		}

		public override SKPath Path(float scale)
		{
			return PathAt(scale, _owner.Width, 0);
		}
	};
}
