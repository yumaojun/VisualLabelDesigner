using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 方形框
	/// </summary>
	public class FrameRect : Frame
	{
		private Distance _w;
		private Distance _h;
		private Distance _r;
		private Distance _xWaste;
		private Distance _yWaste;

		private SkiaSharp.SKPath _path;
		private SkiaSharp.SKPath _clipPath;

		public FrameRect(Distance w, Distance h, Distance r, Distance xWaste, Distance yWaste, string id = "0") : base(id)
		{
			_w = w;
			_h = h;
			_r = r;
			_xWaste = xWaste;
			_yWaste = yWaste;
			_path = new SkiaSharp.SKPath();
			_clipPath = new SkiaSharp.SKPath();
			var rect = new SkiaSharp.SKRect(0, 0, _w.Pt(), _h.Pt());
			_path.AddRoundRect(rect, _r.Pt(), _r.Pt());
			var clipRect = new SkiaSharp.SKRect(-_xWaste.Pt(), -_yWaste.Pt(), -_xWaste.Pt() + _w.Pt() + 2 * _xWaste.Pt(), -_yWaste.Pt() + _h.Pt() + 2 * _yWaste.Pt());
			_clipPath.AddRoundRect(clipRect, _r.Pt(), _r.Pt());
		}

		public FrameRect(FrameRect other) : this(other._w, other._h, other._r, other._xWaste, other._yWaste, other.Id)
		{
			foreach (var item in other.Layouts())
			{
				AddLayout(item);
			}
			foreach (var item in other.Markups())
			{
				AddMarkup(item);
			}
		}

		/// <summary>
		/// 克隆
		/// </summary>
		/// <returns></returns>
		public override Frame Clone() => new FrameRect(this);

		public Distance R() => _r;

		public Distance XWaste() => _xWaste;

		public Distance YWaste() => _yWaste;

		public override Distance Width { get => _w; }

		public override Distance Height { get => _h; set => _h = value; }

		public override bool IsSimilarTo(Frame other)
		{
			var otherRect = other as FrameRect;
			if (otherRect != null)
			{
				if ((Distance.Abs(_w - otherRect._w) <= Constants.EPSILON) &&
					(Distance.Abs(_h - otherRect._h) <= Constants.EPSILON))
				{
					return true;
				}
			}
			return false;
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
			Distance w = _w - 2 * xSize;
			Distance h = _h - 2 * ySize;
			Distance r = Distance.Max(_r - Distance.Min(xSize, ySize), new Distance(0.0f));
			SkiaSharp.SKPath path = new SkiaSharp.SKPath();
			var rect = new SkiaSharp.SKRect(xSize.Pt(), ySize.Pt(), xSize.Pt() + w.Pt(), ySize.Pt() + h.Pt());
			path.AddRoundRect(rect, r.Pt(), r.Pt());
			return path;
		}
	}
}
