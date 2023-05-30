using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 间距
	/// </summary>
	public struct Distance : IEquatable<Distance>
	{
		public float DPts { get; private set; }

		public Distance(float d = 0F)
		{
			DPts = d;
		}

		public Distance(float d, UnitEnum unitEnum = UnitEnum.PT)
		{
			switch (unitEnum)
			{
				case UnitEnum.IN:
					DPts = d * Constants.PTS_PER_INCH;
					break;
				case UnitEnum.MM:
					DPts = d * Constants.PTS_PER_MM;
					break;
				case UnitEnum.CM:
					DPts = d * Constants.PTS_PER_CM;
					break;
				case UnitEnum.PC:
					DPts = d * Constants.PTS_PER_PICA;
					break;
				case UnitEnum.PT:
				default:
					DPts = d;
					break;
			}
		}

		public Distance(float d, Units units) : this(d, units.EnumValue)
		{
		}

		public Distance(float d, string unitsId) : this(d, new Units(unitsId).EnumValue)
		{
		}

		/// <summary>
		/// 点阵
		/// </summary>
		/// <param name="dPts"></param>
		/// <returns></returns>
		public static Distance Pt(float dPts) => new Distance() { DPts = dPts };

		/// <summary>
		/// 英寸
		/// </summary>
		/// <param name="dInches"></param>
		/// <returns></returns>
		public static Distance In(float dInches) => new Distance() { DPts = dInches * Constants.PTS_PER_INCH };

		/// <summary>
		/// 毫米
		/// </summary>
		/// <param name="dMm"></param>
		/// <returns></returns>
		public static Distance Mm(float dMm) => new Distance() { DPts = dMm * Constants.PTS_PER_MM };

		/// <summary>
		/// 厘米
		/// </summary>
		/// <param name="dCm"></param>
		/// <returns></returns>
		public static Distance Cm(float dCm) => new Distance() { DPts = dCm * Constants.PTS_PER_CM };

		/// <summary>
		/// 皮卡=定义为1英尺的 1/72 ，即 4.233 mm (0.166 in)
		/// </summary>
		/// <param name="dPicas"></param>
		/// <returns></returns>
		public static Distance Pc(float dPicas) => new Distance() { DPts = dPicas * Constants.PTS_PER_PICA };

		public float Pt() => DPts;

		public float In() => DPts / Constants.PTS_PER_INCH;

		public float Mm() => DPts / Constants.PTS_PER_MM;

		public float Cm() => DPts / Constants.PTS_PER_CM;

		public float Pc() => DPts / Constants.PTS_PER_PICA;

		public float InUnits(Units units) => InUnits(units.EnumValue);

		public float InUnits(string unitsId) => InUnits(new Units(unitsId));

		public float InUnits(UnitEnum unitEnum)
		{
			switch (unitEnum)
			{
				case UnitEnum.IN:
					return In();
				case UnitEnum.MM:
					return Mm();
				case UnitEnum.CM:
					return Cm();
				case UnitEnum.PC:
					return Pc();
				case UnitEnum.PT:
				default:
					return Pt();
			}
		}

		public static Distance Abs(Distance d) => Distance.Pt(Math.Abs(d.DPts));

		public static Distance Min(Distance d1, Distance d2) => (d1.DPts < d2.DPts) ? d1 : d2;

		public static Distance Max(Distance d1, Distance d2) => (d1.DPts > d2.DPts) ? d1 : d2;

		public static Distance Mod(Distance d1, Distance d2) => Distance.Pt((float)Math.IEEERemainder(d1.DPts, d2.DPts)); // TODO: *存疑=fmod()?

		public bool Equals(Distance other) => DPts.Equals(other.DPts);
		public override bool Equals(object obj) => obj is Distance other && Equals(other);

		public override int GetHashCode() => DPts.GetHashCode();
		public override string ToString() => DPts.ToString();

		public static implicit operator Distance(float v) => new Distance(v);
		public static implicit operator float(Distance d) => d.DPts;

		public static Distance operator +(Distance d) => d;
		public static Distance operator -(Distance d) => new Distance(-d.DPts);

		public static Distance operator +(Distance left, Distance right) => Distance.Pt(left.DPts + right.DPts);
		public static Distance operator -(Distance left, Distance right) => Distance.Pt(left.DPts - right.DPts);

		public static Distance operator *(float v, Distance d) => Distance.Pt(v * d.DPts);
		public static Distance operator *(Distance d, float v) => Distance.Pt(d.DPts * v);

		public static float operator /(Distance left, Distance right) => left.DPts / right.DPts;
		public static Distance operator /(Distance d, float v) => Distance.Pt(d.DPts / v);

		public static bool operator <(Distance left, Distance right) => left.DPts < right.DPts;
		public static bool operator <=(Distance left, Distance right) => left.DPts <= right.DPts;

		public static bool operator >(Distance left, Distance right) => left.DPts > right.DPts;
		public static bool operator >=(Distance left, Distance right) => left.DPts >= right.DPts;

		public static bool operator ==(Distance left, Distance right) => left.Equals(right);
		public static bool operator !=(Distance left, Distance right) => !(left == right);
	}
}
