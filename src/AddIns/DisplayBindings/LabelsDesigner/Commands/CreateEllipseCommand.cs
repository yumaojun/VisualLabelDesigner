
namespace YProgramStudio.LabelsDesigner.Commands
{
	public class CreateEllipseCommand : BaseIconCommand
	{
		public override void Run()
		{
			GetEditor()?.CreateEllipseMode();
		}
	}
}
