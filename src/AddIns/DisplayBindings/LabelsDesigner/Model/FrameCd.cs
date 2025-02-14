﻿using System;
using System.Collections.Generic;
using SkiaSharp;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// CD框
	/// </summary>
	public class FrameCd : Frame
	{
		private Distance _r1;
		private Distance _r2;
		private Distance _w;
		private Distance _h;
		private Distance _waste;

		private SKPath _path;
		private SKPath _clipPath;

		public FrameCd(Distance r1, Distance r2, Distance w, Distance h, Distance waste, string id) : base(id)
		{
			_r1 = r1;
			_r2 = r2;
			_w = w;
			_h = h;
			_waste = waste;

			Distance wReal = (_w == 0) ? 2 * _r1 : _w;
			Distance hReal = (_h == 0) ? 2 * _r1 : _h;

			// Create path
			{
				// Construct outer subpath (may be clipped if it's a business card CD)
				SKPath outerPath = new SKPath();
				var outerRect = new SKRect((wReal / 2f - r1).Pt(), (hReal / 2f - r1).Pt(), (wReal / 2f - r1).Pt() + (2 * r1.Pt()), (hReal / 2f - r1).Pt() + (2 * r1.Pt()));
				outerPath.AddOval(outerRect);

				SKPath clipPath = new SKPath();
				var clipRect = new SKRect(0, 0, wReal.Pt(), hReal.Pt());
				clipPath.AddRect(clipRect);

				// Add inner subpath
				SKPath subpath = new SKPath();
				var rect = new SKRect((wReal / 2f - r2).Pt(), (hReal / 2f - r2).Pt(), (wReal / 2f - r2).Pt() + (2 * r2.Pt()), (hReal / 2f - r2).Pt() + (2 * r2.Pt()));
				subpath.AddOval(rect);

				// 取交集
				_path = outerPath.Op(clipPath, SKPathOp.Intersect).Op(subpath, SKPathOp.Difference);
			}

			// Create clip path
			{
				Distance r1Clip = _r1 + _waste;
				Distance r2Clip = _r2 - _waste;
				Distance wClip = (_w == 0) ? 2 * r1Clip : _w + 2 * _waste;
				Distance hClip = (_h == 0) ? 2 * r1Clip : _h + 2 * _waste;

				// Construct outer subpath (may be clipped if it's a business card CD)
				SKPath outerPath = new SKPath();
				var outerRect = new SKRect((wReal / 2f - r1Clip).Pt(), (hReal / 2f - r1Clip).Pt(), (wReal / 2f - r1Clip).Pt() + (2 * r1Clip.Pt()), (hReal / 2f - r1Clip).Pt() + (2 * r1Clip.Pt()));
				outerPath.AddOval(outerRect);

				SKPath clipPath = new SKPath();
				var clipRect = new SKRect(-_waste.Pt(), -_waste.Pt(), -_waste.Pt() + wClip.Pt(), -_waste.Pt() + hClip.Pt());
				clipPath.AddRect(clipRect);

				// Add inner subpath
				SKPath subpath = new SKPath();
				var rect = new SKRect((wReal / 2f - r2Clip).Pt(), (hReal / 2f - r2Clip).Pt(), (wReal / 2f - r2Clip).Pt() + (2 * r2Clip.Pt()), (hReal / 2f - r2Clip).Pt() + (2 * r2Clip.Pt()));
				subpath.AddOval(rect);

				// 取交集
				_clipPath = outerPath.Op(clipPath, SKPathOp.Intersect).Op(subpath, SKPathOp.Difference);
			}
		}

		public FrameCd(FrameCd other) : this(other._r1, other._r2, other._w, other._h, other._waste, other.Id)
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

		public override Frame Clone() => new FrameCd(this);

		public Distance R1() => _r1;

		public Distance R2() => _r2;

		public Distance Waste() => _waste;

		public override Distance Width { get => (_w == 0) ? 2 * _r1 : _w; }

		public override Distance Height { get => (_h == 0) ? 2 * _r1 : _h; set => _h = value; }

		public override bool IsSimilarTo(Frame other)
		{
			FrameCd otherCd = other as FrameCd;
			if (otherCd != null)
			{
				if ((Distance.Abs(_w - otherCd._w) <= Constants.EPSILON) &&
					(Distance.Abs(_h - otherCd._h) <= Constants.EPSILON) &&
					(Distance.Abs(_r1 - otherCd._r1) <= Constants.EPSILON) &&
					(Distance.Abs(_r2 - otherCd._r2) <= Constants.EPSILON))
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
				string dStr = StrUtil.FormatFraction(2 * _r1.In());
				string diameter = TranslateHelper.Tr("diameter");
				return $"{dStr} {units.ToName()}{diameter}";
			}
			else
			{
				var dStr = 2 * _r1.InUnits(units);//%.5g
				string diameter = TranslateHelper.Tr("diameter");
				return $"{dStr} {units.ToName()}{diameter}";
			}
		}

		public override SKPath Path()
		{
			return _path;
		}

		public override SKPath ClipPath()
		{
			return _clipPath;
		}

		public override SKPath MarginPath(Distance xSize, Distance ySize)
		{
			Distance size = xSize;
			Distance wReal = (_w == 0) ? 2 * _r1 : _w;
			Distance hReal = (_h == 0) ? 2 * _r1 : _h;
			Distance r1 = _r1 - size;
			Distance r2 = _r2 + size;

			// Construct outer subpath (may be clipped if it's a business card CD)
			SKPath outerPath = new SKPath();
			var outerRect = new SKRect((wReal / 2f - r1).Pt(), (hReal / 2f - r1).Pt(), (wReal / 2f - r1).Pt() + (2 * r1.Pt()), (hReal / 2f - r1).Pt() + (2 * r1.Pt()));
			outerPath.AddOval(outerRect);

			SKPath clipPath = new SKPath();
			var clipRect = new SKRect(size.Pt(), size.Pt(), size.Pt() + (wReal - 2f * size).Pt(), size.Pt() + (hReal - 2f * size).Pt());
			clipPath.AddRect(clipRect);

			// Add inner subpath
			SKPath subpath = new SKPath();
			var rect = new SKRect((wReal / 2f - r2).Pt(), (hReal / 2f - r2).Pt(), (wReal / 2f - r2).Pt() + (2 * r2.Pt()), (hReal / 2f - r2).Pt() + (2 * r2.Pt()));
			subpath.AddOval(rect);

			SKPath path = outerPath.Op(clipPath, SKPathOp.Intersect).Op(subpath, SKPathOp.Difference);

			return path;
		}
	}
}
