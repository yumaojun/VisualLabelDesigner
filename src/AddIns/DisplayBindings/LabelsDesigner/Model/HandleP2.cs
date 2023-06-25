using SkiaSharp;
using YProgramStudio.LabelsDesigner.Labels;

namespace YProgramStudio.LabelsDesigner.Model
{
	///
	/// HandleP2 Class
	///
	public class HandleP2 : Handle
	{
		// Lifecycle Methods
		public HandleP2(ModelObject owner) : base(owner, HoverLocation.P2)
		{
		}

		// Duplication
		public HandleP2 Clone(ModelObject newOwner)
		{
			return new HandleP2(newOwner);
		}

		// Drawing Methods
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
