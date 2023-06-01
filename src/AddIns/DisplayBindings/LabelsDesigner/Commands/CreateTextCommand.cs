
namespace YProgramStudio.LabelsDesigner.Commands
{
	public class CreateTextCommand : BaseIconCommand
	{
		public override void Run()
		{
			GetEditor()?.CreateTextMode();
		}
	}
}
