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
	public class CreateBarcodeCommand : AbstractMenuCommand
	{
		public override void Run()
		{
			var s = Screen.PrimaryScreen;
			var c = SD.Workbench.ActiveViewContent.Control as UserControl;
			if (c != null)
			{
				var table = c.Controls.Find("tableLayoutPanel1", false).First();
				var panel = table.Controls.Find("mainPanel", false).First() as Panel;
				var vRuler = table.Controls.Find("vRuler", false).First();
				var str = $"barcode,Screen:{s.Bounds.Width}, ViewPort(W*H):{c.Width}*{c.Height}, TableLayout(W*H):{table.Width}*{table.Height}," +
					$" mainPanel(W*H):{panel.Width}*{panel.Height}, AutoScrollMinSize:{panel.AutoScrollMinSize.Width},vRuler(W*H):{vRuler.Width*vRuler.Height}";
				MessageService.ShowMessage(str);
			}
		}
	}
}
