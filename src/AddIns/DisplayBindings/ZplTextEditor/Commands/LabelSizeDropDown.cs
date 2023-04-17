using ICSharpCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.ZPLTextEditor
{
	public class LabelSizeDropDown : AbstractMenuCommand
	{
		public override void Run()
		{
			Gui.Form1 f = new Gui.Form1();
			f.Text = "LabelSizeDropDown";
			f.ShowDialog();
		}
	}
}
