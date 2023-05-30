using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 标记-椭圆
	/// </summary>
	public class MarkupEllipse : Markup
	{
		private Distance _x1;
		private Distance _y1;
		private Distance _w;
		private Distance _h;

		public MarkupEllipse(Distance x1, Distance y1, Distance w, Distance h)
		{
			_x1 = x1;
			_y1 = y1;
			_w = w;
			_h = h;
			var rect = new SkiaSharp.SKRect(x1.Pt(), y1.Pt(), x1.Pt() + w.Pt(), y1.Pt() + h.Pt());
			_path.AddOval(rect);
		}

		public Distance X1() { return _x1; }

		public Distance Y1() { return _y1; }

		public Distance W() { return _w; }

		public Distance H() { return _h; }

		public override Markup Clone()
		{
			return new MarkupEllipse(_x1, _y1, _w, _h);
		}
	}
}
