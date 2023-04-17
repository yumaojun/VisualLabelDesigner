using ICSharpCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.ZPLTextEditor
{
	public class LabelSizeOption : AbstractMenuCommand
	{
		public override void Run()
		{
			List<LabelFormat> labelFormats = LabelFormat.DeserializeFromXML();
			Gui.LabelSizeOptionForm f = new Gui.LabelSizeOptionForm(labelFormats);
			f.Text = "LabelSizeOption";
			f.ShowDialog();
		}
	}
}
