using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 椭圆框
	/// </summary>
	public class FrameEllipse : Frame
	{
		private Distance _w;
		private Distance _h;
		private Distance _waste;

		private SkiaSharp.SKPath _path;
		private SkiaSharp.SKPath _clipPath;

		public FrameEllipse(Distance w, Distance h, Distance waste, string id = "0") : base(id)
		{
			_w = w;
			_h = h;
			_waste = waste;
			_path = new SkiaSharp.SKPath();
			_clipPath = new SkiaSharp.SKPath();
			var rect = new SkiaSharp.SKRect(0, 0, _w.Pt(), _h.Pt());
			_path.AddOval(rect);
			var clipRect = new SkiaSharp.SKRect(-_waste.Pt(), -_waste.Pt(), -_waste.Pt() + (_w + 2 * _waste).Pt(), -_waste.Pt() + (_h + 2 * _waste).Pt());
			_clipPath.AddOval(clipRect);
		}

		public FrameEllipse(FrameEllipse other) : this(other._w, other._h, other._waste, other.Id)
		{
			foreach (var item in other.Layouts())
			{
				AddLayout(item);
			}
			foreach (var item in other.Markups())
			{
				AddMarkup(item.Clone());
			}
		}

		public override Frame Clone() => new FrameEllipse(this);

		public override Distance Width { get => _w; }

		public override Distance Height { get => _h; set => _h = value; }

		public override bool IsSimilarTo(Frame other)
		{
			var otherEllipse = other as FrameEllipse;
			if (otherEllipse != null)
			{
				if ((Math.Abs(_w - otherEllipse._w) <= Constants.EPSILON) &&
					(Math.Abs(_h - otherEllipse._h) <= Constants.EPSILON))
				{
					return true;
				}
			}
			return false;
		}

		public override SkiaSharp.SKPath Path()
		{
			return _path;
		}

		public override SkiaSharp.SKPath ClipPath()
		{
			return _clipPath;
		}

		public override SkiaSharp.SKPath MarginPath(Distance xSize, Distance ySize)
		{
			// Note: ignore ySize, assume xSize == ySize
			Distance size = xSize;
			Distance w = _w - 2 * size;
			Distance h = _h - 2 * size;
			SkiaSharp.SKPath path = new SkiaSharp.SKPath();
			var rect = new SkiaSharp.SKRect(size.Pt(), size.Pt(), size.Pt() + w.Pt(), size.Pt() + h.Pt());
			path.AddOval(rect);
			return path;
		}

		public override string SizeDescription(Units units)
		{
			if (units.EnumValue == UnitEnum.IN)
			{
				string wStr = StrUtil.FormatFraction(_w.In());
				string hStr = StrUtil.FormatFraction(_h.In());
				return $"{wStr} x {hStr} {units.ToName()}";
			}
			else
			{
				var wStr = _w.InUnits(units);
				var hStr = _h.InUnits(units);
				return $"{wStr}g x {hStr}g {units.ToName()}"; // "%.5g x %.5g %s"
			}
		}
	}
}
