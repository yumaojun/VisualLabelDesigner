using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 圆角矩形框
	/// </summary>
	public class FrameRound : Frame
	{
		private Distance _r;
		private Distance _waste;

		private SkiaSharp.SKPath _path;
		private SkiaSharp.SKPath _clipPath;

		public FrameRound(Distance r, Distance waste, string id = "0") : base(id)
		{
			_r = r;
			_waste = waste;
			_path = new SkiaSharp.SKPath();
			_clipPath = new SkiaSharp.SKPath();
			var rect = new SkiaSharp.SKRect(0, 0, 2 * _r.Pt(), 2 * _r.Pt());
			_path.AddOval(rect);
			var clipRect = new SkiaSharp.SKRect(-_waste.Pt(), -_waste.Pt(),
								  -_waste.Pt() + 2 * (_r + _waste).Pt(), -_waste.Pt() + 2 * (_r + _waste).Pt());
			_clipPath.AddOval(clipRect);
		}

		public FrameRound(FrameRound other) : this(other._r, other._waste, other.Id())
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

		public override Frame Clone() => new FrameRound(this);

		public override Distance Width { get => 2 * _r; }

		public override Distance Height { get => 2 * _r; set => _r = value; }

		public Distance R() => _r;

		public Distance Waste() => _waste;

		public override bool IsSimilarTo(Frame other)
		{
			var otherRound = other as FrameRound;
			if (otherRound != null)
			{
				if (Distance.Abs(_r - otherRound._r) <= Constants.EPSILON)
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
				string dStr = StrUtil.FormatFraction(2 * _r.In());
				string diameter = TranslateHelper.Tr("diameter");
				return $"{dStr} {units.ToName()} {diameter}"; //"%s %s %s"
			}
			else
			{
				var dStr = 2 * _r.InUnits(units);
				string diameter = TranslateHelper.Tr("diameter");
				return $"{dStr}g {units.ToName()} {diameter}"; //"%.5g %s %s"
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
			// Note: ignore ySize, assume xSize == ySize
			Distance size = xSize;
			Distance r = _r - size;
			SkiaSharp.SKPath path = new SkiaSharp.SKPath();
			var rect = new SkiaSharp.SKRect(size.Pt(), size.Pt(), size.Pt() + 2 * r.Pt(), size.Pt() + 2 * r.Pt());
			path.AddOval(rect);
			return path;
		}
	}
}
