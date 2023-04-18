using ICSharpCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YProgramStudio.LabelsDesigner.Gui;

namespace YProgramStudio.LabelsDesigner.Commands
{
	public class CreateEllipseCommand : AbstractMenuCommand
	{
		public override void Run()
		{
			var val = MessageService.ShowInputBox("ellipse", "dialogText..", "test");
			MessageBox.Show(val);
		}
	}
}
