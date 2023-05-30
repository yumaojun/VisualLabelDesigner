using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 标记-外间距
	/// </summary>
	public class MarkupMargin : Markup
	{
		private Distance _xSize;
		private Distance _ySize;

		public MarkupMargin(Distance size)
		{
			_xSize = size;
			_ySize = size;
		}

		public MarkupMargin(Distance xSize, Distance ySize)
		{
			_xSize = xSize;
			_ySize = ySize;
		}

		public Distance XSize() { return _xSize; }

		public Distance YSize() { return _ySize; }

		public override Markup Clone()
		{
			return new MarkupMargin(_xSize, _ySize);
		}

		public override SKPath Path(Frame frame)
		{
			return frame.MarginPath(_xSize, _ySize); // Re-calculate path -- frame size may have changed
		}
	}
}
