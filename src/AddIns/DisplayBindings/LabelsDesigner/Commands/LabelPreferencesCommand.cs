using ICSharpCode.Core;
using ICSharpCode.SharpDevelop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YProgramStudio.LabelsDesigner.Gui;

namespace YProgramStudio.LabelsDesigner.Commands
{
	public class LabelPreferencesCommand : AbstractMenuCommand
	{
		public override void Run()
		{
			LabelReference preferences = new LabelReference();
			preferences.Owner = SD.Workbench.MainWindow;
			preferences.ShowDialog();
		}
	}
}
