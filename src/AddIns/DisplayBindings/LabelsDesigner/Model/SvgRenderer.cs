using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	public class SvgRenderer
	{
		private byte[] _bytes;

		public SvgRenderer(byte[] bytes)
		{
			_bytes = bytes;
		}

		public void Render(SKCanvas painter, SKRect destRect) { 
		
		}

		public SKRect ViewBox()
		{
			return new SKRect();
		}
	}
}
