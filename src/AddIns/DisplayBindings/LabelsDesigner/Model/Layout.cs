using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 布局
	/// </summary>
	public class Layout
	{
		private int _nx;
		private int _ny;
		private Distance _x0;
		private Distance _y0;
		private Distance _dx;
		private Distance _dy;

		public Layout(int nx, int ny, Distance x0, Distance y0, Distance dx, Distance dy)
		{
			_nx = nx;
			_ny = ny;
			_x0 = x0;
			_y0 = y0;
			_dx = dx;
			_dy = dy;
		}

		public Layout(Layout other)
		{
			_nx = other._nx;
			_ny = other._ny;
			_x0 = other._x0;
			_y0 = other._y0;
			_dx = other._dx;
			_dy = other._dy;
		}

		public int Nx() { return _nx; }
		public int Ny() { return _ny; }

		public Distance X0() { return _x0; }
		public Distance Y0() { return _y0; }

		public Distance Dx() { return _dx; }
		public Distance Dy() { return _dy; }

		public bool IsSimilarTo(Layout other)
		{
			return (_nx == other._nx) &&
				(_ny == other._ny) &&
				(Distance.Abs(_x0 - other._x0) < Constants.EPSILON) &&
				(Distance.Abs(_y0 - other._y0) < Constants.EPSILON) &&
				(Distance.Abs(_dx - other._dx) < Constants.EPSILON) &&
				(Distance.Abs(_dy - other._dy) < Constants.EPSILON);
		}
	}
}
