using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YProgramStudio.LabelsDesigner.Labels;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// Handle Base Class
	/// </summary>
	public abstract class Handle
	{
		protected const float handlePixels = 7F;
		protected const float handleOutlineWidthPixels = 1F;

		protected static readonly SKColor handleFillColor = System.Drawing.Color.FromArgb(96, 0, 192, 0).ToSKColor(); // argb
		protected static readonly SKColor originHandleFillColor = System.Drawing.Color.FromArgb(96, 192, 0, 0).ToSKColor();
		protected static readonly SKColor handleOutlineColor = System.Drawing.Color.FromArgb(192, 0, 0, 0).ToSKColor();

		protected ModelObject _owner;
		protected HoverLocation _location;

		public ModelObject Owner
		{
			get => _owner;
		}

		public HoverLocation Location
		{
			get => _location;
		}

		/// <summary>
		/// Handle Constructor
		/// </summary>
		public Handle(ModelObject owner, HoverLocation location)
		{
			_owner = owner;
			_location = location;
		}

		#region Drawing Methods

		public abstract void Draw(SKCanvas painter, float scale);

		public abstract SKPath Path(float scale);

		// Draw Handle at x,y
		protected void DrawAt(SKCanvas painter, float scale, Distance x, Distance y, SKColor color)
		{
			painter.Save();

			painter.Translate(x.Pt(), y.Pt());

			float s = 1.0F / scale;

			using (SKPaint paint = new SKPaint()
			{
				Color = color,
				Style = SKPaintStyle.Fill,
				IsAntialias = true
			})
			{
				painter.DrawRect(-s * handlePixels / 2.0F, -s * handlePixels / 2.0F, s * handlePixels, s * handlePixels, paint);
				paint.Color = handleOutlineColor;
				paint.Style = SKPaintStyle.Stroke;
				paint.StrokeWidth = handleOutlineWidthPixels;
				painter.DrawRect(-s * handlePixels / 2.0F, -s * handlePixels / 2.0F, s * handlePixels, s * handlePixels, paint);
			}

			painter.Restore();
		}

		// Create Handle path at x,y
		protected SKPath PathAt(float scale, Distance x, Distance y)
		{
			SKPath path = new SKPath();

			float s = 1f / scale;

			SKRect rect = new SKRect(-s * handlePixels / 2, -s * handlePixels / 2, s * handlePixels, s * handlePixels);

			path.AddRect(rect);
			//path.Translate(x.Pt(), y.Pt()); // TODO: 位移？

			SKPath path1 = new SKPath();
			path1.AddPath(path, x.Pt(), y.Pt());

			return path1;
		}

		#endregion
	}
}
