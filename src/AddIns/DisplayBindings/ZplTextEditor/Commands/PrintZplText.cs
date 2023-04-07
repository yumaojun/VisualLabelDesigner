using ICSharpCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastColoredTextBoxNS;
using ICSharpCode.SharpDevelop;
using ICSharpCode.SharpDevelop.Workbench;

namespace VisualLabelDesigner.ZplTextEditor
{
	/// <summary>
	/// Print zpl text
	/// </summary>
	public class PrintZplText : AbstractMenuCommand
	{
		public override void Run()
		{
			IViewContent vc = SD.Workbench.ActiveViewContent;
			if (vc != null)
			{
				ZplTextEditorPanel zplTextEditor = vc.Control as ZplTextEditorPanel;
				if (zplTextEditor != null)
				{
					PrintDialogSettings printDialogSettings = new PrintDialogSettings();
					printDialogSettings.IncludeLineNumbers = true;
					printDialogSettings.Header = vc.TitleName;
					zplTextEditor.ZplCodeTextBox.Print(printDialogSettings);
				}
			}
		}
	}
}
