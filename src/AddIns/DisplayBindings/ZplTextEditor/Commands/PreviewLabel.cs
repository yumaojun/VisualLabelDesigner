using ICSharpCode.Core;
using ICSharpCode.SharpDevelop;
using ICSharpCode.SharpDevelop.Workbench;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YProgramStudio.ZPLTextEditor.Gui;

namespace YProgramStudio.ZPLTextEditor
{
	/// <summary>
	/// preview label & send to labelary.com
	/// </summary>
	public class PreviewLabel : AbstractMenuCommand
	{
		public override void Run()
		{
			IViewContent vc = SD.Workbench.ActiveViewContent;

			if (vc != null)
			{
				ZPLTextEditorPanel zplTextEditor = vc.Control as ZPLTextEditorPanel;
				LabelaryDpmm ldpmm = PrinterOrCanvasComboBox.Instance.GetValue();
				LabelFormat lf = LabelSizeComboBox.Instance.GetValue();

				if (ldpmm == null)
				{
					MessageService.ShowMessage("Please Select LabelaryDpmm");
					return;
				}

				if (lf == null)
				{
					MessageService.ShowMessage("Please Select LabelSize");
					return;
				}

				if (zplTextEditor != null && CommandPreview.Instance != null)
				{
					CommandPreview.Instance.Render(zplTextEditor.ZplCodeTextBox.Text, lf, ldpmm);
				}
			}
		}
	}
}
