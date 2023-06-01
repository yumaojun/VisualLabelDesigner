
namespace YProgramStudio.LabelsDesigner.Commands
{
	public class ZoomOneToOneCommand : BaseIconCommand
	{
		public override void Run()
		{
			GetEditor()?.Zoom1To1();
		}
	}
}
