using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	///
	/// HandleEast Class
	///
	public class HandleEast : Handle
	{
		// Lifecycle Methods
		public HandleEast(ModelObject owner) : base(owner, Labels.Location.E) { }

		public HandleEast Clone(ModelObject newOwner)
		{
			return new HandleEast(newOwner);
		}

		// Drawing Methods
		public override void Draw(SKCanvas painter, float scale) { DrawAt(painter, scale, _owner.Width, _owner.Height / 2f, handleFillColor); }

		public override SKPath Path(float scale)
		{
			return PathAt(scale, _owner.Width, _owner.Height / 2f);
		}
	};
}
