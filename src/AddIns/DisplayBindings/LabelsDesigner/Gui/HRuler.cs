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
	/// horizontal ruler
	/// </summary>
	public class HRuler : SKControl
	{
		public HRuler()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
            this.SuspendLayout();
            // 
            // HRuler
            // 
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Size = new System.Drawing.Size(10000, 27);
            this.PaintSurface += new System.EventHandler<SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs>(this.HRuler_PaintSurface);
            this.ResumeLayout(false);

		}

		private void HRuler_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
		{
			int dpmm = 6;
			int dpcm = dpmm * 10;
			int startOrigin = 0;
			int numberLineHeight = 14; // 字高
			int lineWidth = 10;
			int sortLineWidth = 5;
			int numberCount = 0;
			var canvas = e.Surface.Canvas;

			SKPaint paint = new SKPaint() { Color = SKColors.Black, Style = SKPaintStyle.Stroke, StrokeWidth = 1F };
			for (int i = startOrigin; i < Width;)
			{
				canvas.DrawLine(i, 0 + numberLineHeight, i, lineWidth + numberLineHeight, paint);
				canvas.DrawText(numberCount.ToString(), i, 10f, paint);
				int splitPostion = 1;
				for (int j = dpmm + i; j < dpcm + i;)
				{
					if (splitPostion % 5 == 0)
					{
						canvas.DrawLine(j, sortLineWidth + numberLineHeight, j, lineWidth + numberLineHeight, paint);
					}
					else
					{
						canvas.DrawLine(j, sortLineWidth * 2 + numberLineHeight, j, lineWidth + numberLineHeight, paint);
					}
					j += dpmm;
					++splitPostion;
				}
				i += dpcm;
				++numberCount;
			}
			canvas.DrawLine(0, lineWidth + numberLineHeight, Width, lineWidth + numberLineHeight, paint);
			paint.Dispose();
		}
	}
}
