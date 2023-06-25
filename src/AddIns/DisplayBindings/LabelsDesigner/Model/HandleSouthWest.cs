using SkiaSharp;
using YProgramStudio.LabelsDesigner.Labels;

namespace YProgramStudio.LabelsDesigner.Model
{
	///
	/// HandleSouthWest Class
	///
	public class HandleSouthWest : Handle
	{
		// Lifecycle Methods
		public HandleSouthWest(ModelObject owner) : base(owner, HoverLocation.SW)
		{
		}

		public HandleSouthWest Clone(ModelObject newOwner)
		{
			return new HandleSouthWest(newOwner);
		}

		// Drawing Methods
		public override void Draw(SKCanvas painter, float scale)
		{
			DrawAt(painter, scale, 0, _owner.Height, handleFillColor);
		}

		public override SKPath Path(float scale)
		{
			return PathAt(scale, 0, _owner.Height);
		}
	};

}
