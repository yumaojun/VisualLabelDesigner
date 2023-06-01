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
		private float _x0;
		private float _y0;

		/// <summary>
		/// 标尺原点x坐标
		/// </summary>
		public float X0 { get => _x0; set { _x0 = value; Refresh(); } }

		/// <summary>
		/// 标尺原点y坐标
		/// </summary>
		public float Y0 { get => _y0; set { _y0 = value; Refresh(); } }

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
			this.BackColor = System.Drawing.Color.Lavender;
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Size = new System.Drawing.Size(24, 1000);
			this.PaintSurface += new System.EventHandler<SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs>(this.VRuler_PaintSurface);
			this.ResumeLayout(false);

		}

		public void UpdateRuler(float? x0, float? y0)
		{
			_x0 = x0 ?? _x0;
			_y0 = y0 ?? _y0;
			Refresh();
		}

		private void VRuler_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
		{
			var canvas = e.Surface.Canvas;

			int dpmm = 6; // dot 每毫米像素数
			int dpcm = dpmm * 10; // 1厘米
			int numberLineHeight = 14; // 字高
			int lineWidth = 10;
			int sortLineWidth = 5;
			int numberCount = 0;

			float rulerWidth = 23f; // 尺的宽度

			canvas.Clear();

			SKPaint paint = new SKPaint() { Color = SKColors.Black, Style = SKPaintStyle.Stroke, StrokeWidth = 1F };
			for (float i = _y0; i < Height;)
			{
				canvas.DrawLine(0, i, rulerWidth, i, paint);
				using (SKFontStyle fontStyle = new SKFontStyle(SKFontStyleWeight.Thin, SKFontStyleWidth.Condensed, SKFontStyleSlant.Upright))
				using (SKTypeface typeface = SKTypeface.FromFamilyName("Simsun", fontStyle))
				using (SKPaint textPaint = new SKPaint()
				{
					Color = SKColors.Black,
					IsAntialias = true,
					Style = SKPaintStyle.Stroke,
					StrokeWidth = 1f,
					Typeface = typeface
				})
				{
					string numberText = numberCount.ToString();
					SKRect rect = new SKRect();
					textPaint.MeasureText(numberText, ref rect);
					float x0 = (Width - rect.Width) / 2;
					float y0 = rect.Height + 2f;
					canvas.DrawText(numberText, x0, i + y0, textPaint);
				}
				int splitPostion = 1;
				for (float j = dpmm + i; j < dpcm + i;)
				{
					if (splitPostion % 5 == 0)
					{
						canvas.DrawLine(lineWidth - (sortLineWidth * 2) + numberLineHeight, j, rulerWidth, j, paint);
					}
					else
					{
						canvas.DrawLine(lineWidth - sortLineWidth + numberLineHeight, j, rulerWidth, j, paint);
					}
					j += dpmm;
					++splitPostion;
				}
				i += dpcm;
				++numberCount;
			}
			canvas.DrawLine(rulerWidth, 0, rulerWidth, Height, paint);
			paint.Dispose();
		}
	}
}
