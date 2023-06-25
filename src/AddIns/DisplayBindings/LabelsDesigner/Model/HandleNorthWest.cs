using SkiaSharp;
using YProgramStudio.LabelsDesigner.Labels;

namespace YProgramStudio.LabelsDesigner.Model
{
	///
	/// HandleNorthWest Class
	///
	public class HandleNorthWest : Handle
	{
		// Lifecycle Methods
		public HandleNorthWest(ModelObject owner) : base(owner, HoverLocation.NW)
		{
		}

		public HandleNorthWest Clone(ModelObject newOwner)
		{
			return new HandleNorthWest(newOwner);
		}

		// Drawing Methods
		public override void Draw(SKCanvas painter, float scale)
		{
			DrawAt(painter, scale, 0f, 0f, originHandleFillColor);
		}

		public override SKPath Path(float scale)
		{
			return PathAt(scale, 0f, 0f);
		}
	};
}
