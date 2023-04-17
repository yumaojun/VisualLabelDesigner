using ICSharpCode.Core;
using ICSharpCode.SharpDevelop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YProgramStudio.ZPLTextEditor.Services;

namespace YProgramStudio.ZPLTextEditor
{
	public class VariableMappingCommand : AbstractMenuCommand
	{
		public override void Run()
		{
			string zplcode = string.Empty;
			ZPLTextEditorPanel zplTextEditorPanel = SD.Workbench.ActiveViewContent?.Control as ZPLTextEditorPanel;
			if (zplTextEditorPanel != null)
			{
				zplcode = zplTextEditorPanel.ZplCodeTextBox.Text;
			}
			Dictionary<string, string> variables = VariableSubstitution.Load();
			Gui.VariableMappingForm f = new Gui.VariableMappingForm(zplcode, variables);
			f.Text = "VariableMapping";
			f.ShowDialog();
		}
	}
}
