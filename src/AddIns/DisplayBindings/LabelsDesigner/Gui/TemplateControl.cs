using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YProgramStudio.LabelsDesigner.Labels;
using YProgramStudio.LabelsDesigner.Model;

namespace YProgramStudio.LabelsDesigner.Gui
{
	/// <summary>
	/// 标签模板显示控件
	/// </summary>
	public class TemplateControl : SkiaSharp.Views.Desktop.SKControl
	{
		private static SKColor paperColor = System.Drawing.Color.FromArgb(217, 217, 217).ToSKColor();
		private static SKColor paperOutlineColor = System.Drawing.Color.Black.ToSKColor();
		private const float paperOutlineWidthPixels = 1.0f;

		private static SKColor labelColor = System.Drawing.Color.FromArgb(255, 250, 250).ToSKColor();
		private static SKColor labelOutlineColor = System.Drawing.Color.FromArgb(64, 64, 64).ToSKColor();
		private const float labelOutlineWidthPixels = 1.0f;

		private Template _template;

		public TemplateControl()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// TemplateControl
			// 
			this.PaintSurface += new System.EventHandler<SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs>(this.TemplateControl_PaintSurface);
			this.ResumeLayout(false);
		}

		public void UpdateView(Template template)
		{
			_template = template;
			Refresh();
		}

		private void TemplateControl_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs e)
		{
			if (_template == null)
			{
				return;
			}

			e.Surface.Canvas.Clear();

			// 对于“滚动”模板，为胶带宽度和连续折线留出额外空间
			Distance drawWidth = _template.PageWidth;
			Distance drawHeight = _template.PageHeight;
			if (_template.IsRoll)
			{
				drawWidth = _template.RollWidth;
				drawHeight *= 1.2f;
			}

			int width = 260, height = 260;
			float w = width - 1;
			float h = height - 1;
			float scale;

			if ((w / drawWidth.Pt()) > (h / drawHeight.Pt()))
			{
				scale = h / drawHeight.Pt();
			}
			else
			{
				scale = w / drawWidth.Pt();
			}

			e.Surface.Canvas.Scale(scale, scale); // 将当前矩阵与指定的比例预先连接

			Distance xOffset = (Distance.Pt(width / scale) - _template.PageWidth) / 2f;
			Distance yOffset = (Distance.Pt(height / scale) - _template.PageHeight) / 2f;
			e.Surface.Canvas.Translate(xOffset.Pt(), yOffset.Pt()); // 将当前矩阵与指定的平移预连接
			//e.Surface.Canvas.Translate(5, 5);

			DrawPaper(e.Surface, scale);
			DrawLabelOutlines(e.Surface, scale);
		}

		private void DrawPaper(SkiaSharp.SKSurface sf, float scale)
		{
			sf.Canvas.Save();

			var paint = new SkiaSharp.SKPaint()
			{
				Style = SKPaintStyle.Stroke,
				Color = paperOutlineColor,
				StrokeWidth = paperOutlineWidthPixels / scale,
				IsAntialias = true
			};

			if (!_template.IsRoll)
			{
				sf.Canvas.DrawRect(0, 0, _template.PageWidth.Pt(), _template.PageHeight.Pt(), paint);
				paint.Style = SKPaintStyle.Fill;
				paint.Color = paperColor;
				sf.Canvas.DrawRect(0, 0, _template.PageWidth.Pt(), _template.PageHeight.Pt(), paint);
			}
			else
			{
				sf.Canvas.DrawPath(new RollTemplatePath(_template), paint);
				paint.Style = SKPaintStyle.Fill;
				paint.Color = paperColor;
				sf.Canvas.DrawPath(new RollTemplatePath(_template), paint);
			}

			sf.Canvas.Restore();
		}

		private void DrawLabelOutlines(SkiaSharp.SKSurface sf, float scale)
		{
			var paint = new SkiaSharp.SKPaint()
			{
				StrokeWidth = paperOutlineWidthPixels / scale,
				IsAntialias = true
			};

			sf.Canvas.Save();

			Frame frame = _template.Frames.First();
			Point[] origins = frame.GetOrigins();

			foreach (Point p0 in origins)
			{
				DrawLabelOutline(sf, frame, p0, paint);
			}

			sf.Canvas.Restore();
		}

		private void DrawLabelOutline(SkiaSharp.SKSurface sf, Frame frame, Point p0, SkiaSharp.SKPaint paint)
		{
			sf.Canvas.Save();
			sf.Canvas.Translate(p0.X().Pt(), p0.Y().Pt());
			paint.Style = SKPaintStyle.Stroke;
			paint.Color = labelOutlineColor;
			sf.Canvas.DrawPath(frame.Path(), paint);
			paint.Style = SKPaintStyle.Fill;
			paint.Color = labelColor;
			sf.Canvas.DrawPath(frame.Path(), paint);
			sf.Canvas.Restore();
		}
	}
}
