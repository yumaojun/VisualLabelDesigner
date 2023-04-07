
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualLabelDesigner.ZplTextEditor.Services;

namespace VisualLabelDesigner.ZplTextEditor.Gui
{
	public class EventArgsZPLCommand
	{
		public ZPLCommand zplCommand { get; set; }

		public EventArgsZPLCommand(ZPLCommand zc)
		{
			this.zplCommand = zc;
		}
	}
}
