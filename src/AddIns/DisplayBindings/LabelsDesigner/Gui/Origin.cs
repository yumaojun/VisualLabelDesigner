using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Gui
{
	public class Origin : SKControl
	{
		public Origin()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
            this.SuspendLayout();
            // 
            // Origin
            // 
            this.BackColor = System.Drawing.Color.Lavender;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Size = new System.Drawing.Size(24, 24);
            this.PaintSurface += new System.EventHandler<SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs>(this.Origin_PaintSurface);
            this.ResumeLayout(false);

		}

		private void Origin_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
		{
			var canvas = e.Surface.Canvas;

			SKTypeface typeface =  SKTypeface.FromFamilyName("SimSun");

			using (SKPaint paint = new SKPaint()
			{
				Color = SKColors.Black,
				Style = SKPaintStyle.Stroke,
				StrokeWidth = 1.5f,
				Typeface = typeface
			})
			{
				string str = "+";
				SKRect rect = new SKRect();
				paint.MeasureText(str, ref rect);
				float x0 = (Width - rect.Width) / 2;
				float y0 = (Height - rect.Height) / 2 + rect.Height; // 字的y坐标是以字的基线为准
				canvas.DrawText(str, x0, y0, paint);
			}
		}
	}
}
