using ICSharpCode.Core;
using ICSharpCode.SharpDevelop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YProgramStudio.ZPLTextEditor.Gui;

namespace YProgramStudio.ZPLTextEditor
{
	public class PrintLabel : AbstractMenuCommand
	{
		public override void Run()
		{
			//Form1 f = new Form1();
			//f.Text = "PrintLabel";
			//f.ShowDialog();

			var zplCodeControl = SD.Workbench.ActiveViewContent.Control as ZPLTextEditor.ZPLTextEditorPanel;
			string text = ZPLCode.Clean(zplCodeControl.ZplCodeTextBox.Text);
			//RawPrinterHelper.SendStringToPrinter(this.cbLocalPrinter.Text, text);


		}
	}
}
