using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 路径框
	/// </summary>
	public class FramePath : Frame
	{
		private Distance _w;
		private Distance _h;
		private Distance _xWaste;
		private Distance _yWaste;

		private SkiaSharp.SKPath _path;
		private SkiaSharp.SKPath _clipPath;

		private Units _originalUnits;

		public FramePath(SkiaSharp.SKPath path, Distance xWaste, Distance yWaste, Units originalUnits, string id) : base(id)
		{
			var r = path.Bounds;
			_w = Distance.Pt(r.Width);
			_h = Distance.Pt(r.Height);
			_xWaste = xWaste;
			_yWaste = yWaste;
			_path = path;
			_originalUnits = originalUnits;

			_clipPath = new SkiaSharp.SKPath();
			var clipRect = new SkiaSharp.SKRect(r.Left - _xWaste.Pt(), r.Top - _yWaste.Pt(), r.Right + 2 * _xWaste.Pt(), r.Bottom + 2 * _yWaste.Pt());
			_clipPath.AddRect(clipRect);
		}

		// 复制构造要怎么写？
		public FramePath(FramePath other) : base(other)
		{
			var r = other._path.Bounds;
			_w = other._w;
			_h = other._h;
			_xWaste = other._xWaste;
			_yWaste = other._yWaste;
			_path = new SkiaSharp.SKPath();
			_originalUnits = other._originalUnits;
			_clipPath = new SkiaSharp.SKPath();
			var clipRect = new SkiaSharp.SKRect(r.Left - _xWaste.Pt(), r.Top - _yWaste.Pt(), r.Right + 2 * _xWaste.Pt(), r.Bottom + 2 * _yWaste.Pt());
			_clipPath.AddRect(clipRect);
		}

		public override Frame Clone() => new FramePath(this);

		public override Distance Width { get => _w; }

		public override Distance Height { get => _h; set => _h = value; }

		public Distance XWaste() => _xWaste;

		public Distance YWaste() => _yWaste;

		public Units OriginalUnits() => _originalUnits;

		public override bool IsSimilarTo(Frame other)
		{
			var otherPath = other as FramePath;
			if (otherPath != null)
			{
				if (_path == otherPath._path) // TODO: 两个path怎么判断相等（是比较路径是否一样，复杂路径比较耗时）
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
				return $"{wStr} x {hStr} {units.ToName()}"; // "%.5g x %.5g %s"
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
			return _path; // No margin
		}
	}
}
