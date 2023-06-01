using ICSharpCode.Core;
using ICSharpCode.SharpDevelop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YProgramStudio.LabelsDesigner.Gui;

namespace YProgramStudio.LabelsDesigner.Commands
{
	public abstract class BaseIconCommand : AbstractMenuCommand
	{
		protected LabelEditor GetEditor()
		{
			LabelEditor labelEditor = null;
			LabelViewContent viewContent = SD.Workbench.ActiveViewContent as LabelViewContent;
			if (viewContent != null)
			{
				LabelDesignerPanel designerPanel = viewContent.Control as LabelDesignerPanel;
				var tableLayoutPanel = designerPanel.Controls.Find("tableLayoutPanel1", false).First();
				var mainPanel = tableLayoutPanel.Controls.Find("mainPanel", false).First() as Panel;
				labelEditor = mainPanel.Controls.Find("labelEditor", false).First() as LabelEditor;
			}
			return labelEditor;
		}
	}
}
