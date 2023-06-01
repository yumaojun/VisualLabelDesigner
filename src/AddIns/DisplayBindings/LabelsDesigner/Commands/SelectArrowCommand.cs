
namespace YProgramStudio.LabelsDesigner.Commands
{
	public class SelectArrowCommand : BaseIconCommand
	{
		public override void Run()
		{
			GetEditor()?.ArrowMode();
		}
	}
}
