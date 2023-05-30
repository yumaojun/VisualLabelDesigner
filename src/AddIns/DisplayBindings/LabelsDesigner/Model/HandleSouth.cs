using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	///
	/// HandleSouth Class
	///
	public class HandleSouth : Handle
	{
		// Lifecycle Methods
		public HandleSouth(ModelObject owner) : base(owner, Labels.Location.S) { }

		public HandleSouth Clone(ModelObject newOwner)
		{
			return new HandleSouth(newOwner);
		}

		// Drawing Methods
		public override void Draw(SKCanvas painter, float scale) { DrawAt(painter, scale, _owner.Width / 2f, _owner.Height, handleFillColor); }

		public override SKPath Path(float scale)
		{
			return PathAt(scale, _owner.Width / 2f, _owner.Height);
		}
	};
}
