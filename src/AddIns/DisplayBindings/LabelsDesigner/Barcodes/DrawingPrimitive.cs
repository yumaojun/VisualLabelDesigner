using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Barcodes
{
	public class DrawingPrimitive
	{
		private float _x;
		private float _y;

		public float X { get => _x; set => _x = value; }
		public float Y { get => _y; set => _y = value; }

		public DrawingPrimitive(float x, float y)
		{
			_x = x;
			_y = y;
		}
	}
}
