using ICSharpCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.ZPLTextEditor
{
	/// <summary>
	/// Connect Zebra Printer
	/// </summary>
	public class ConnectPrinter : AbstractMenuCommand
	{
		public override void Run()
		{
			Gui.Form1 f = new Gui.Form1();
			f.Text = "ConnectPrinter";
			f.ShowDialog();
		}
	}
}
