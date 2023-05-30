using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YProgramStudio.LabelsDesigner.Model;

namespace YProgramStudio.LabelsDesigner.Labels
{
	public delegate void PointerMoveEventHandler(object sender, PointerMoveEventArgs e);

	public class PointerMoveEventArgs : EventArgs
	{
		public Distance XWorld { get; set; }

		public Distance YWorld { get; set; }

		public PointerMoveEventArgs(Distance xWorld, Distance yWorld)
		{
			XWorld = xWorld;
			YWorld = yWorld;
		}
	}
}
