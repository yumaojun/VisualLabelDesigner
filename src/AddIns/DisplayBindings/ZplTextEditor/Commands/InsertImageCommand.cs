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
using VisualLabelDesigner.ZplTextEditor.Services;

namespace VisualLabelDesigner.ZplTextEditor.Commands
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
				ZplImageConverter zplImageConverter = new ZplImageConverter();
				zplImageConverter.Image = Image.FromFile(openFileDialog.FileName);
				zplImageConverter.CompressHex = true;
				zplImageConverter.BlacknessLimitPercentage = 50;

				IViewContent vc = SD.Workbench.ActiveViewContent;
				if (vc != null)
				{
					ZplTextEditorPanel zplTextEditor = vc.Control as ZplTextEditorPanel;
					zplTextEditor.ZplCodeTextBox.SelectedText = zplImageConverter.Value;
				}
			}
		}
	}
}
