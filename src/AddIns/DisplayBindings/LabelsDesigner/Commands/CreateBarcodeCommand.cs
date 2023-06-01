
namespace YProgramStudio.LabelsDesigner.Commands
{
	public class CreateBarcodeCommand : BaseIconCommand
	{
		public override void Run()
		{
			GetEditor()?.CreateBarcodeMode();
		}
	}
}
