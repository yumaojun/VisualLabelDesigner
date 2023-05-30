using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 标记-圆
	/// </summary>
	public class MarkupCircle : Markup
	{
		private Distance _x0;
		private Distance _y0;
		private Distance _r;

		public MarkupCircle(Distance x0, Distance y0, Distance r) {
			_x0 = x0;
			_y0 = y0;
			_r = r;
			_path.AddCircle(_x0.Pt(), _y0.Pt(), r.Pt());
		}

		public Distance X0() { return _x0; }

		public Distance Y0() { return _x0; }

		public Distance R() { return _x0; }

		public override Markup Clone()
		{
			return new MarkupCircle(_x0, _y0, _r);
		}
	}
}
