using SkiaSharp;
using YProgramStudio.LabelsDesigner.Labels;

namespace YProgramStudio.LabelsDesigner.Model
{
	///
	/// HandleP1 Class
	///
	public class HandleP1 : Handle
	{
		// Lifecycle Methods
		public HandleP1(ModelObject owner) : base(owner, HoverLocation.P1)
		{
		}

		public HandleP1 Clone(ModelObject newOwner)
		{
			return new HandleP1(newOwner);
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
