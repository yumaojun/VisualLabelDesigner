using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 选中对象时的线框
	/// </summary>
	public class Outline
	{
		const float dashSize = 1F;
		const float slopPixels = 2F;
		const float outlineWidthPixels = 0f; // TODO：默认是1F，设置为0是为了使线宽不受放大缩小变换的影响
		static readonly SKColor outlineColor1 = System.Drawing.Color.FromArgb(0, 0, 0).ToSKColor();
		static readonly SKColor outlineColor2 = System.Drawing.Color.FromArgb(255, 255, 255).ToSKColor();

		private ModelObject _owner;
		private float[] _dashes;
		private SKPaint _paint1;
		private SKPaint _paint2;

		public Outline(ModelObject owner)
		{
			_owner = owner;
			_dashes = new[] { dashSize, dashSize };
			//SKMaskFilter f = SKMaskFilter.CreateBlur(SKBlurStyle.Outer, 0.5f);
			_paint1 = new SKPaint()
			{
				Color = outlineColor1,
				Style = SKPaintStyle.Stroke,
				StrokeWidth = outlineWidthPixels,
				StrokeCap = SKStrokeCap.Butt,
				PathEffect = SKPathEffect.CreateDash(_dashes, 0f)//,
				//MaskFilter = f
			};
			_paint2 = new SKPaint()
			{
				Color = outlineColor2,
				Style = SKPaintStyle.Stroke,
				StrokeWidth = outlineWidthPixels,
				StrokeCap = SKStrokeCap.Butt,
				PathEffect = SKPathEffect.CreateDash(_dashes, dashSize)
			};
		}

		public Outline(Outline outline, ModelObject newOwner)
		{
			_owner = newOwner;// TODO: *Outline的构造函数入参newOwner需要是复制吗？
			_dashes = outline._dashes;
			_paint1 = outline._paint1;
			_paint2 = outline._paint2;
		}

		~Outline()
		{
			_paint1?.Dispose();
			_paint2?.Dispose();
		}

		// Drawing Methods
		public void Draw(SkiaSharp.SKCanvas painter)
		{
			painter.Save();

			painter.DrawRect(0, 0, _owner.Width.Pt(), _owner.Height.Pt(), _paint1);
			painter.DrawRect(0, 0, _owner.Width.Pt(), _owner.Height.Pt(), _paint2);

			painter.Restore();
		}

		/// <summary>
		/// Create path for testing for hover condition
		/// </summary>
		public SKPath HoverPath(float scale)
		{
			float s = 1 / scale;

			SKPath path = new SKPath();

			SKRect rect = new SKRect(-s * slopPixels, -s * slopPixels, _owner.Width.Pt() + s * 2 * slopPixels, _owner.Height.Pt() + s * 2 * slopPixels);
			path.AddRect(rect);
			path.Close();

			SKRect rect1 = new SKRect(s * slopPixels, s * slopPixels, _owner.Width.Pt() - s * 2 * slopPixels, _owner.Height.Pt() - s * 2 * slopPixels);
			path.AddRect(rect1);

			return path;
		}
	}
}
