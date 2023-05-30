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
	/// 连续的框
	/// </summary>
	public class FrameContinuous : Frame
	{
		private Distance _w;
		private Distance _h;
		private Distance _hMin;
		private Distance _hMax;
		private Distance _hDefault;

		private SkiaSharp.SKPath _path;

		public FrameContinuous(Distance w, Distance hMin, Distance hMax, Distance hDefault, string id) : base(id)
		{
			_w = w;
			_h = new Distance(0);
			_hMin = hMin;
			_hMax = hMax;
			_hDefault = hDefault;
			_path = new SkiaSharp.SKPath();
			var rect = new SkiaSharp.SKRect(0, 0, _w.Pt(), _h.Pt());
			_path.AddRect(rect);
		}

		public FrameContinuous(FrameContinuous other) : this(other._w, other._hMin, other._hMax, other._hDefault, other.Id())
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

		public override Frame Clone() => new FrameContinuous(this);

		public override Distance Width { get => _w; }

		public override Distance Height
		{
			get => _h;
			set
			{
				_h = value;
				_path?.Dispose();
				_path = null;
				_path = new SkiaSharp.SKPath(); // clear path
				var rect = new SkiaSharp.SKRect(0, 0, _w.Pt(), _h.Pt());
				_path.AddRect(rect);
			}
		}

		public override bool IsSimilarTo(Frame other)
		{
			var otherContinuous = other as FrameContinuous;
			if (otherContinuous != null)
			{
				if (Math.Abs(_w - otherContinuous._w) <= Constants.EPSILON)
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
				string wideTr = TranslateHelper.Tr("wide");
				return $"{wStr} {units.ToName()} {wideTr}";
			}
			else
			{
				var wStr = _w.InUnits(units);
				string wideTr = TranslateHelper.Tr("wide");
				return $"{wStr:N3} {units.ToName()} {wideTr}"; // "%.3f %s %s"
			}
		}

		public override SkiaSharp.SKPath Path()
		{
			return _path;
		}

		public override SkiaSharp.SKPath ClipPath()
		{
			return _path;
		}

		public override SkiaSharp.SKPath MarginPath(Distance xSize, Distance ySize)
		{
			Distance w = _w - 2 * xSize;
			Distance h = _h - 2 * ySize;
			SkiaSharp.SKPath path = new SkiaSharp.SKPath();
			var rect = new SkiaSharp.SKRect(xSize.Pt(), ySize.Pt(), xSize.Pt() + w.Pt(), ySize.Pt() + h.Pt());
			path.AddRect(rect);
			return path;
		}
	}
}
