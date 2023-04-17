using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.ZPLTextEditor.Gui
{
	public class EventArgsString : EventArgs
	{
		public string MyEventString { get; set; }

		public EventArgsString(string myString)
		{
			this.MyEventString = myString;
		}
	}
}
