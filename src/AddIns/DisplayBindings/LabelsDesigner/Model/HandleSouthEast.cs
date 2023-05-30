using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	///
	/// HandleSouthEast Class
	///
	public class HandleSouthEast : Handle
	{
		// Lifecycle Methods
		public HandleSouthEast(ModelObject owner) : base(owner, Labels.Location.SE) { }

		public HandleSouthEast Clone(ModelObject newOwner)
		{
			return new HandleSouthEast(newOwner);
		}

		////////////////////////////
		// Drawing Methods
		////////////////////////////
		public override void Draw(SKCanvas painter, float scale) { DrawAt(painter, scale, _owner.Width, _owner.Height, handleFillColor); }

		public override SKPath Path(float scale)
		{
			return PathAt(scale, _owner.Width, _owner.Height);
		}
	};
}
