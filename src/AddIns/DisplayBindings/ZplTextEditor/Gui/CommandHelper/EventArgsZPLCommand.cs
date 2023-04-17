
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YProgramStudio.ZPLTextEditor.Services;

namespace YProgramStudio.ZPLTextEditor.Gui
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
