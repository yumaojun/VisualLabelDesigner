
namespace YProgramStudio.LabelsDesigner.Commands
{
	public class ZoomOutCommand : BaseIconCommand
	{
		public override void Run()
		{
			GetEditor()?.ZoomOut();
		}
	}
}
