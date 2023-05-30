using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Gui
{
	/// <summary>
	/// vertical ruler
	/// </summary>
	public class VRuler : SKControl
	{
		public VRuler()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// VRuler
			// 
			this.BackColor = System.Drawing.SystemColors.InactiveCaption;
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Size = new System.Drawing.Size(27, 10000);
			this.PaintSurface += new System.EventHandler<SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs>(this.VRuler_PaintSurface);
			this.ResumeLayout(false);

		}

		private void VRuler_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
		{
			int dpmm = 6; // dot 每毫米像素数
			int dpcm = dpmm * 10;
			int startOrigin = 0;
			int numberLineHeight = 15; // 字高
			int lineWidth = 10;
			int sortLineWidth = 5;
			int numberCount = 0;
			var canvas = e.Surface.Canvas;

			SKPaint paint = new SKPaint() { Color = SKColors.Black, Style = SKPaintStyle.Stroke, StrokeWidth = 1F };
			for (int i = startOrigin; i < Height;)
			{
				canvas.DrawLine(0 + numberLineHeight, i, lineWidth + numberLineHeight, i, paint);
				canvas.DrawText(numberCount.ToString(), 0, i + 10f, paint);
				int splitPostion = 1;
				for (int j = dpmm + i; j < dpcm + i;)
				{
					if (splitPostion % 5 == 0)
					{
						canvas.DrawLine(lineWidth - (sortLineWidth * 2) + numberLineHeight, j, lineWidth + numberLineHeight, j, paint);
					}
					else
					{
						canvas.DrawLine(lineWidth - sortLineWidth + numberLineHeight, j, lineWidth + numberLineHeight, j, paint);
					}
					j += dpmm;
					++splitPostion;
				}
				i += dpcm;
				++numberCount;
			}
			canvas.DrawLine(lineWidth + numberLineHeight, 0, lineWidth + numberLineHeight, Height, paint);
			paint.Dispose();
		}
	}
}
