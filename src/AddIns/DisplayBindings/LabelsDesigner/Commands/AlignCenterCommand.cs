using ICSharpCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YProgramStudio.LabelsDesigner.Gui;

namespace YProgramStudio.LabelsDesigner.Commands
{
	public class AlignCenterCommand : BaseIconCommand
	{
		public override void Run()
		{
			MessageService.ShowMessage("AlignCenter");
		}
	}
}
