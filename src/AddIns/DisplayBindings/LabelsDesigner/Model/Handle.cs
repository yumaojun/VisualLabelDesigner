// Copyright (c) 2023 余茂军 <yumaojun@gmail.com> All rights reserved.

using SkiaSharp;
using SkiaSharp.Views.Desktop;
using YProgramStudio.LabelsDesigner.Labels;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// Handle Base Class
	/// </summary>
	public abstract class Handle
	{
		protected const float handlePixels = 7F; // Handle的方框大小
		protected const float handleOutlineWidthPixels = 0; // Handle的线框宽度1Fpix

		protected static readonly SKColor handleFillColor = System.Drawing.Color.FromArgb(96, 0, 192, 0).ToSKColor(); // argb
		protected static readonly SKColor originHandleFillColor = System.Drawing.Color.FromArgb(96, 192, 0, 0).ToSKColor();
		protected static readonly SKColor handleOutlineColor = System.Drawing.Color.FromArgb(192, 0, 0, 0).ToSKColor();

		protected ModelObject _owner;
		protected HoverLocation _location;

		public ModelObject Owner => _owner;

		public HoverLocation Location => _location;

		/// <summary>
		/// Handle Constructor
		/// </summary>
		public Handle(ModelObject owner, HoverLocation location)
		{
			_owner = owner;
			_location = location;
		}

		#region Drawing Methods

		/// <summary>
		/// Handle draw 锚点Handle绘制
		/// </summary>
		/// <param name="painter"></param>
		/// <param name="scale"></param>
		public abstract void Draw(SKCanvas painter, float scale);

		/// <summary>
		/// Handle path 锚点Handle路径
		/// </summary>
		/// <param name="scale"></param>
		/// <returns></returns>
		public abstract SKPath Path(float scale);

		/// <summary>
		/// Draw Handle at x,y 在x, y绘制锚点Handle
		/// </summary>
		/// <param name="painter">canvas 画布</param>
		/// <param name="scale">scale 缩放因子</param>
		/// <param name="x">x 偏移坐标</param>
		/// <param name="y">y 偏移坐标</param>
		/// <param name="color">color 颜色</param>
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

		/// <summary>
		/// Create Handle path at x,y 在位置x, y创建锚点Handle路径
		/// </summary>
		/// <param name="scale">scale 缩放因子</param>
		/// <param name="x">x 偏移坐标</param>
		/// <param name="y">y 偏移坐标</param>
		/// <returns></returns>
		protected SKPath PathAt(float scale, Distance x, Distance y)
		{
			SKPath path = new SKPath();

			float s = 1f / scale;

			SKRect rect = new SKRect(-s * handlePixels / 2, -s * handlePixels / 2, s * handlePixels, s * handlePixels);
			path.AddRect(rect);

			SKMatrix matrix = SKMatrix.CreateTranslation(x.Pt(), y.Pt());
			path.Transform(matrix);

			return path;
		}

		#endregion
	}
}
