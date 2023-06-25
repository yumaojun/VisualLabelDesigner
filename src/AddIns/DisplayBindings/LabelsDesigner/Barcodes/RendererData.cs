using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Barcodes
{
	public struct RendererData
	{
		public SKCanvas Painter { get; set; }
		public SKColor Color { get; set; }
	}
}
