using ICSharpCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YProgramStudio.ZPLTextEditor.Gui;

namespace YProgramStudio.ZPLTextEditor
{
	public class PrinterOrCanvasDropDown : AbstractMenuCommand
	{
		public override void Run()
		{
			Form1 f = new Form1();
			f.Text = "PrinterOrCanvasDropDown";
			f.ShowDialog();
		}
	}
}
