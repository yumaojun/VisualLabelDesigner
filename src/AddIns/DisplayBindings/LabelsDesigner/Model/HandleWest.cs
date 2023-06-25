using SkiaSharp;
using YProgramStudio.LabelsDesigner.Labels;

namespace YProgramStudio.LabelsDesigner.Model
{
	///
	/// HandleWest Class
	///
	public class HandleWest : Handle
	{
		// Lifecycle Methods
		public HandleWest(ModelObject owner) : base(owner, HoverLocation.W)
		{
		}

		public HandleWest Clone(ModelObject newOwner)
		{
			return new HandleWest(newOwner);
		}

		// Drawing Methods
		public override void Draw(SKCanvas painter, float scale)
		{
			DrawAt(painter, scale, 0, _owner.Height / 2f, handleFillColor);
		}

		public override SKPath Path(float scale)
		{
			return PathAt(scale, 0, _owner.Height / 2f);
		}
	};
}
