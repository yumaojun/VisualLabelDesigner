
namespace YProgramStudio.LabelsDesigner.Commands
{
	public class CreateBoxCommand : BaseIconCommand
	{
		public override void Run()
		{
			GetEditor()?.CreateBoxMode();
		}
	}
}
