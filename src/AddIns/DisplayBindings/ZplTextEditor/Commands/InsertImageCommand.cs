using ICSharpCode.Core;
using ICSharpCode.SharpDevelop;
using ICSharpCode.SharpDevelop.Workbench;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YProgramStudio.ZPLTextEditor.Services;

namespace YProgramStudio.ZPLTextEditor.Commands
{
	public class InsertImageCommand : AbstractMenuCommand
	{
		public override void Run()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "PNG|*.png|JPEG|*.jpg *.jpeg *.jpe *.jfif|Bitmap|.bmp|All file|*.*";
			openFileDialog.Title = "Open Image...";
			openFileDialog.ShowDialog();
			if (!string.IsNullOrEmpty(openFileDialog.FileName))
			{
				ZPLImageConverter zplImageConverter = new ZPLImageConverter();
				zplImageConverter.Image = Image.FromFile(openFileDialog.FileName);
				zplImageConverter.CompressHex = true;
				zplImageConverter.BlacknessLimitPercentage = 50;

				IViewContent vc = SD.Workbench.ActiveViewContent;
				if (vc != null)
				{
					ZPLTextEditorPanel zplTextEditor = vc.Control as ZPLTextEditorPanel;
					zplTextEditor.ZplCodeTextBox.SelectedText = zplImageConverter.Value;
				}
			}
		}
	}
}
