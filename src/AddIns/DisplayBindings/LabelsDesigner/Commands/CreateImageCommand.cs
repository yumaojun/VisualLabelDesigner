
namespace YProgramStudio.LabelsDesigner.Commands
{
	public class CreateImageCommand : BaseIconCommand
	{
		public override void Run()
		{
			//OpenFileDialog openFileDialog = new OpenFileDialog();
			//openFileDialog.Filter = "PNG|*.png|JPEG|*.jpg *.jpeg *.jpe *.jfif|Bitmap|.bmp|All file|*.*";
			//openFileDialog.Title = "Open Image...";
			//openFileDialog.ShowDialog();
			GetEditor()?.CreateImageMode();
		}
	}
}
