
namespace YProgramStudio.LabelsDesigner.Commands
{
	public class ZoomToFitCommand : BaseIconCommand
	{
		public override void Run()
		{
			GetEditor()?.ZoomToFit();
		}
	}
}
