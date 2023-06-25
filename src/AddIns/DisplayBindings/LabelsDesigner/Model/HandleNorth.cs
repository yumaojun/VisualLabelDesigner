using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YProgramStudio.LabelsDesigner.Labels;

namespace YProgramStudio.LabelsDesigner.Model
{
	///
	/// HandleNorth Class
	///
	public class HandleNorth : Handle
	{
		// Lifecycle Methods
		public HandleNorth(ModelObject owner) : base(owner, HoverLocation.N)
		{
		}

		public HandleNorth Clone(ModelObject newOwner)
		{
			return new HandleNorth(newOwner);
		}

		// Drawing Methods
		public override void Draw(SKCanvas painter, float scale)
		{
			DrawAt(painter, scale, _owner.Width / 2f, 0, handleFillColor);
		}

		public override SKPath Path(float scale)
		{
			return PathAt(scale, _owner.Width / 2f, 0);
		}
	};
}
