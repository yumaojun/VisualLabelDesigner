
namespace YProgramStudio.LabelsDesigner.Commands
{
	public class ZoomInCommand : BaseIconCommand
	{
		public override void Run()
		{
			GetEditor()?.ZoomIn();
		}
	}
}
