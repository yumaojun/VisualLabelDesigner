using ICSharpCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualLabelDesigner.ZplTextEditor
{
	/// <summary>
	/// Select Printer or Canvas
	/// </summary>
	public class PrinterDropDown : AbstractMenuCommand
	{
		public override void Run()
		{
			Gui.Form1 f = new Gui.Form1();
			f.Text = "PrintDropDown";
			f.ShowDialog();
		}
	}
}
