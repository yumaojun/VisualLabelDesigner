using SkiaSharp;
using YProgramStudio.LabelsDesigner.Model;

namespace YProgramStudio.LabelsDesigner.Labels
{
	public class PreviewOverlayItem
	{
		private PageRenderer _renderer;

		public PageRenderer Renderer { get => _renderer; set => _renderer = value; }

		public PreviewOverlayItem(PageRenderer renderer)
		{
			this.Renderer = renderer;
		}

		public SKRect BoundingRect
		{
			get => Renderer.PageRect;
		}

		public void Paint(SKCanvas painter)
		{
			_renderer.PrintPage(painter);
		}
	}
}