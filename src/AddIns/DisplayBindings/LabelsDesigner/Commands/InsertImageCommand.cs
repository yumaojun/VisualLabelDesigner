using ICSharpCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YProgramStudio.LabelsDesigner.Gui;

namespace YProgramStudio.LabelsDesigner.Commands
{
	public class InsertImageCommand : AbstractMenuCommand
	{
		public override void Run()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "PNG|*.png|JPEG|*.jpg *.jpeg *.jpe *.jfif|Bitmap|.bmp|All file|*.*";
			openFileDialog.Title = "Open Image...";
			openFileDialog.ShowDialog();
		}
	}
}
