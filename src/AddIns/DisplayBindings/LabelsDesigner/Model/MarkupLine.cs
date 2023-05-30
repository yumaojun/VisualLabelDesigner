using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 标记-线
	/// </summary>
	public class MarkupLine : Markup
	{
		private Distance _x1;
		private Distance _y1;
		private Distance _x2;
		private Distance _y2;

		public MarkupLine(Distance x1, Distance y1, Distance x2, Distance y2)
		{
			_x1 = x1;
			_y1 = y1;
			_x2 = x2;
			_y2 = y2;
			_path.MoveTo(x1.Pt(), y1.Pt());
			_path.LineTo(x2.Pt(), y2.Pt());
		}

		public Distance X1() { return _x1; }

		public Distance Y1() { return _y1; }

		public Distance X2() { return _x2; }

		public Distance Y2() { return _y2; }

		public override Markup Clone()
		{
			return new MarkupLine(_x1, _y1, _x2, _y2);
		}
	}
}
