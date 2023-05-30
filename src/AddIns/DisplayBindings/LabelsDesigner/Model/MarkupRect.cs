using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 标记-圆角方形
	/// </summary>
	public class MarkupRect: Markup
	{
		private Distance _x1;
		private Distance _y1;
		private Distance _w;
		private Distance _h;
		private Distance _r;

		public MarkupRect(Distance x1, Distance y1, Distance w, Distance h, Distance r) 
		{
			_x1 = x1;
			_y1 = y1;
			_w = w;
			_h = h;
			_r = r;
			var rect = new SkiaSharp.SKRect(x1.Pt(), y1.Pt(), x1.Pt() + w.Pt(), y1.Pt() + h.Pt());
			_path.AddRoundRect(rect, r.Pt(), r.Pt());
		}

		public Distance X1() { return _x1; }
		public Distance Y1() { return _y1; }
		public Distance W() { return _w; }
		public Distance H() { return _h; }
		public Distance R() { return _r; }

		public override Markup Clone()
		{
			return new MarkupRect(_x1, _y1, _w, _h, _r);
		}
	}
}
