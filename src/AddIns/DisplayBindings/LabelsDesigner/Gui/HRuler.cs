using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YProgramStudio.LabelsDesigner.Model;

namespace YProgramStudio.LabelsDesigner.Gui
{
	/// <summary>
	/// horizontal ruler
	/// </summary>
	public class HRuler : SKControl
	{
		private float _x0;
		private float _y0;

		/// <summary>
		/// 标尺原点x坐标
		/// </summary>
		public float X0
		{
			get => _x0;
			set { _x0 = value; Refresh(); }
		}

		/// <summary>
		/// 标尺原点y坐标
		/// </summary>
		public float Y0
		{
			get => _y0;
			set { _y0 = value; Refresh(); }
		}

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
			this.BackColor = System.Drawing.Color.Lavender;
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Size = new System.Drawing.Size(1000, 24);
			this.PaintSurface += new System.EventHandler<SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs>(this.HRuler_PaintSurface);
			this.ResumeLayout(false);

		}

		public void UpdateRuler(float? x0, float? y0)
		{
			_x0 = x0 ?? _x0;
			_y0 = y0 ?? _y0;
			Refresh();
		}

		private void HRuler_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
		{
			var canvas = e.Surface.Canvas;

			int dpmm = 6;
			int dpcm = dpmm * 10;
			int numberLineHeight = 14; // 字高
			int lineWidth = 10;
			int sortLineWidth = 5;
			int numberCount = 0;

			float rulerWidth = 23f; // 尺的宽度

			canvas.Clear();

			SKPaint paint = new SKPaint() { Color = SKColors.Black, Style = SKPaintStyle.Stroke, StrokeWidth = 1F };

			SKFontStyle fontStyle = new SKFontStyle(SKFontStyleWeight.Thin, SKFontStyleWidth.Condensed, SKFontStyleSlant.Upright);
			SKTypeface typeface = SKTypeface.FromFamilyName("Simsun", fontStyle);
			SKPaint textPaint = new SKPaint()
			{
				Color = SKColors.Black,
				IsAntialias = true,
				Style = SKPaintStyle.Stroke,
				StrokeWidth = 1f,
				Typeface = typeface
			};

			// 绘制大于0的部分
			int splitPostion = 1;

			for (float i = _x0; i < Width; i += dpcm)
			{
				canvas.DrawLine(i, 0, i, rulerWidth, paint);

				string numberText = numberCount.ToString();
				SKRect rect = new SKRect();
				textPaint.MeasureText(numberText, ref rect);
				float x0 = 2f;
				float y0 = (Height - rect.Height) / 2 + rect.Height - 2f;

				if (i + dpcm < Width)
					canvas.DrawText(numberText, i + x0, y0, textPaint);

				for (float j = dpmm + i; j < dpcm + i; j += dpmm)
				{
					if (splitPostion % 5 == 0)
					{
						canvas.DrawLine(j, lineWidth - (sortLineWidth * 2) + numberLineHeight, j, rulerWidth, paint);
					}
					else
					{
						canvas.DrawLine(j, lineWidth - sortLineWidth + numberLineHeight, j, rulerWidth, paint);
					}

					++splitPostion;
				}

				++numberCount;
			}

			// 绘制-1和0之间的部分
			splitPostion = 1;

			for (float i = _x0 - dpmm; i > (_x0 - dpmm * 10); i -= dpmm)
			{
				if (splitPostion % 5 == 0)
				{
					canvas.DrawLine(i, lineWidth - (sortLineWidth * 2) + numberLineHeight, i, rulerWidth, paint);
				}
				else
				{
					canvas.DrawLine(i, lineWidth - sortLineWidth + numberLineHeight, i, rulerWidth, paint);
				}

				++splitPostion;
			}

			// 绘制小于-1的部分
			numberCount = -1;

			for (float i = _x0 - dpcm; i > 0; i -= dpcm)
			{
				canvas.DrawLine(i, 0, i, rulerWidth, paint);

				string numberText = numberCount.ToString();
				SKRect rect = new SKRect();
				textPaint.MeasureText(numberText, ref rect);
				float x0 = 2f;
				float y0 = (Height - rect.Height) / 2 + rect.Height - 2f;

				canvas.DrawText(numberText, i + x0, y0, textPaint);

				splitPostion = 1;

				for (float j = i - dpmm; j > i - dpcm; j -= dpmm)
				{
					if (splitPostion % 5 == 0)
					{
						canvas.DrawLine(j, lineWidth - (sortLineWidth * 2) + numberLineHeight, j, rulerWidth, paint);
					}
					else
					{
						canvas.DrawLine(j, lineWidth - sortLineWidth + numberLineHeight, j, rulerWidth, paint);
					}

					++splitPostion;
				}

				--numberCount;
			}

			canvas.DrawLine(0, rulerWidth, Width, rulerWidth, paint);

			string rule = "毫米"; //TranslateHelper.Tr("mm");
			SKRect ruleRrect = new SKRect();
			textPaint.MeasureText(rule, ref ruleRrect);
			float ruleX0 = Width - 30f;
			float ruleY0 = (Height - ruleRrect.Height) / 2f + ruleRrect.Height - 2f;
			//SKRect clipRect = new SKRect(ruleX0 - 1f, ruleY0 - ruleRrect.Height - 1f, ruleRrect.Width + 1f, ruleRrect.Height + 1f);
			//canvas.ClipRect(clipRect);
			//canvas.Clear();
			canvas.DrawText(rule, ruleX0, ruleY0, textPaint);

			textPaint.Dispose();
			typeface.Dispose();
			fontStyle.Dispose();
			paint.Dispose();
		}
	}
}
