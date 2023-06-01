using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YProgramStudio.LabelsDesigner.Model;

namespace YProgramStudio.LabelsDesigner.Labels
{
	public delegate void ZoomChangeEventHandler(object sender, ZoomChangeEventArgs e);

	public class ZoomChangeEventArgs : EventArgs
	{
		public float X { get; set; }

		public float Y { get; set; }

		public ZoomChangeEventArgs(float x, float y)
		{
			X = x;
			Y = y;
		}
	}
}
