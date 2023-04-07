using ICSharpCode.Core;
using ICSharpCode.SharpDevelop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualLabelDesigner.ZplTextEditor.Services;

namespace VisualLabelDesigner.ZplTextEditor
{
	public class VariableMappingCommand : AbstractMenuCommand
	{
		public override void Run()
		{
			string zplcode = string.Empty;
			ZplTextEditorPanel zplTextEditorPanel = SD.Workbench.ActiveViewContent?.Control as ZplTextEditorPanel;
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
