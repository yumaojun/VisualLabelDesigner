using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 选区
	/// </summary>
	public class Region
	{
		private Distance _x1;
		private Distance _y1;
		private Distance _x2;
		private Distance _y2;

		public Distance X1 { get => _x1; set => _x1 = value; }
		public Distance Y1 { get => _y1; set => _y1 = value; }
		public Distance X2 { get => _x2; set => _x2 = value; }
		public Distance Y2 { get => _y2; set => _y2 = value; }

		public SKRect Rect
		{
			get
			{
				var x = Distance.Min(_x1, _x2).Pt();
				var y = Distance.Min(_y1, _y2).Pt();
				var width = Distance.Abs(_x2 - _x1).Pt();
				var height = Distance.Abs(_y2 - _y1).Pt();
				return new SKRect(x, y, x + width, y + height);
			}
		}
	}
}
