using SkiaSharp;
using YProgramStudio.LabelsDesigner.Labels;

namespace YProgramStudio.LabelsDesigner.Model
{
	///
	/// HandleSouthEast Class
	///
	public class HandleSouthEast : Handle
	{
		// Lifecycle Methods
		public HandleSouthEast(ModelObject owner) : base(owner, HoverLocation.SE)
		{
		}

		public HandleSouthEast Clone(ModelObject newOwner)
		{
			return new HandleSouthEast(newOwner);
		}

		////////////////////////////
		// Drawing Methods
		////////////////////////////
		public override void Draw(SKCanvas painter, float scale)
		{
			DrawAt(painter, scale, _owner.Width, _owner.Height, handleFillColor);
		}

		public override SKPath Path(float scale)
		{
			return PathAt(scale, _owner.Width, _owner.Height);
		}
	};
}
