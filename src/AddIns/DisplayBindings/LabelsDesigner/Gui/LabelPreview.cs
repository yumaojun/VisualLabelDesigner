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
	/// 打印、打印预览
	/// </summary>
	public class LabelPreview : SkiaSharp.Views.WPF.SKElement
	{
		#region Private Data

		private readonly SKColor paperColor = System.Drawing.Color.FromArgb(242, 242, 242).ToSKColor();
		private readonly SKColor paperOutlineColor = System.Drawing.Color.FromArgb(0, 0, 0).ToSKColor();
		private const float paperOutlineWidthPixels = 1f;

		private readonly SKColor shadowColor = System.Drawing.Color.FromArgb(128, 64, 64, 64).ToSKColor();
		private const float shadowOffsetPixels = 3;
		private const float shadowRadiusPixels = 12;

		private readonly SKColor labelColor = System.Drawing.Color.FromArgb(255, 255, 255).ToSKColor();
		private readonly SKColor labelOutlineColor = System.Drawing.Color.FromArgb(192, 192, 192).ToSKColor();
		private const float labelOutlineWidthPixels = 1;

		private readonly SKColor labelNumberColor = System.Drawing.Color.FromArgb(128, 192, 192, 255).ToSKColor();
		private readonly string labelNumberFontFamily = "SimSun"; // Sans
		private const float labelNumberScale = 0.5f;

		private Model.Model _model;
		private PageRenderer _renderer;

		#endregion

		public PageRenderer Renderer
		{
			get => _renderer;
			set
			{
				_renderer = value;
				_renderer.Changed += new EventHandler(OnRendererChanged);
				OnRendererChanged(null, null);
				//InvalidateVisual();
			}
		}

		/// <summary>
		/// Draw
		/// </summary>
		protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
		{
			var surface = e.Surface;
			var canvas = surface.Canvas;

			DrawPaper(canvas);
			DrawLabels(canvas);
			DrawPreviewOverlay(canvas);
			DrawLabelNumberOverlay(canvas);
		}

		#region Internal Methods

		private void Clear()
		{
		}

		private void OnRendererChanged(object sender, EventArgs e)
		{
			_model = Renderer.Model;

			Clear();

			if (_model != null)
			{
				var tmplate = _model.Template;

				// For "Roll" templates, allow extra room to draw continuation break lines.
				Distance drawHeight = _model.Template.PageHeight;
				Distance drawOffset = 0;
				if (tmplate.IsRoll)
				{
					drawHeight = 1.2f * tmplate.PageHeight;
					drawOffset = 0.1f * tmplate.PageHeight;
				}

				// Set scene up with a 5% margin around paper
				Distance x = -0.05f * tmplate.PageWidth;
				Distance y = -0.05f * drawHeight - drawOffset;
				Distance w = 1.10f * tmplate.PageWidth;
				Distance h = 1.10f * drawHeight;

				Width = w;
				Height = h;

				MinWidth = w;
				MinHeight = h;

				//mScene->setSceneRect(x.pt(), y.pt(), w.pt(), h.pt());
				//fitInView(mScene->sceneRect(), Qt::KeepAspectRatio);

				//DrawPaper();
				//DrawLabels();
				//DrawPreviewOverlay();
				//DrawLabelNumberOverlay();
			}
		}

		private void DrawPaper(SKCanvas painter)
		{
			painter.Save();

			using (SKPaint paint = new SKPaint()
			{
				Color = shadowColor,
				Style = SKPaintStyle.Fill,
				IsAntialias = true
			})
			{
				var tmplate = _model.Template;
				if (!tmplate.IsRoll)
				{
					var pageItem = new SKRect(0, 0, tmplate.PageWidth.Pt(), tmplate.PageHeight.Pt());
					// Draw shadow
					painter.Save();
					painter.Translate(shadowOffsetPixels, shadowOffsetPixels); // shadowRadiusPixels
					painter.DrawRect(pageItem, paint);
					painter.Restore();
					// Draw paper
					paint.Color = paperColor;
					painter.DrawRect(pageItem, paint);
					paint.Color = paperOutlineColor;
					paint.Style = SKPaintStyle.Stroke;
					paint.StrokeWidth = paperOutlineWidthPixels;
					painter.DrawRect(pageItem, paint);
				}
				else
				{
					var pageItem = new SKPath(new RollTemplatePath(tmplate));
					// Draw shadow
					painter.Save();
					painter.Translate(shadowRadiusPixels, shadowOffsetPixels);
					painter.DrawPath(pageItem, paint);
					painter.Restore();
					// Draw paper
					painter.DrawPath(pageItem, paint);
				}
			}

			painter.Restore();
		}

		private void DrawLabels(SKCanvas painter)
		{
			Frame frame = _model.Template.Frames.First();

			foreach (Model.Point origin in frame.GetOrigins())
			{
				DrawLabel(painter, origin.X, origin.Y, frame.Path());
			}
		}

		private void DrawLabel(SKCanvas painter, Distance x, Distance y, SKPath path)
		{
			painter.Save();

			using (SKPaint paint = new SKPaint()
			{
				Color = labelColor,
				Style = SKPaintStyle.Fill,
				StrokeWidth = labelOutlineWidthPixels,
				IsAntialias = true
			})
			{
				painter.Translate(x.Pt(), y.Pt());
				painter.DrawPath(path, paint);
				paint.Color = labelOutlineColor;
				paint.Style = SKPaintStyle.Stroke;
				painter.DrawPath(path, paint);
			}

			painter.Restore();
		}

		private void DrawPreviewOverlay(SKCanvas painter)
		{
			painter.Save();

			if (_renderer != null)
			{
				var overlayItem = new PreviewOverlayItem(_renderer);
				overlayItem.Paint(painter);
			}

			painter.Restore();
		}

		private void DrawLabelNumberOverlaySingle(SKCanvas painter, Distance x, Distance y, SKPath path, uint labelInstance)
		{
			painter.Save();

			Frame frame = _model.Template.Frames.First();

			Distance w = frame.Width;
			Distance h = frame.Height;

			Distance minWH = Math.Min(w, h);

			var labelText = labelInstance.ToString();

			using (SKTypeface typeface = SKTypeface.FromFamilyName(labelNumberFontFamily, SKFontStyle.Bold))
			using (SKPaint paint = new SKPaint()
			{
				Color = labelNumberColor,
				Style = SKPaintStyle.StrokeAndFill,
				TextSize = minWH.Pt() * labelNumberScale * 1.25f, // todo: calc by dpi 像素需要换算为磅, 文本位置也需要修正
				Typeface = typeface,
				IsAntialias = true
			})
			{
				SKRect rect = new SKRect();
				paint.MeasureText(labelText, ref rect);
				painter.DrawText(labelText, (x + w / 2f).Pt() - (rect.Width / 2), (y + h / 2f).Pt() + (rect.Height / 2), paint);
			}

			painter.Restore();
		}

		private void DrawLabelNumberOverlay(SKCanvas painter)
		{
			Frame frame = _model.Template.Frames.First();
			uint i = 0;

			foreach (Point origin in frame.GetOrigins())
			{
				DrawLabelNumberOverlaySingle(painter, origin.X, origin.Y, frame.Path(), ++i);
			}
		}

		#endregion
	}
}
