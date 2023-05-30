using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	public class Units
	{
		public readonly static Units Pt = new Units(UnitEnum.PT);
		public readonly static Units In = new Units(UnitEnum.IN);
		public readonly static Units Mm = new Units(UnitEnum.MM);
		public readonly static Units Cm = new Units(UnitEnum.CM);
		public readonly static Units Pc = new Units(UnitEnum.PC);

		public UnitEnum EnumValue { get; private set; }

		public Units()
		{
			EnumValue = UnitEnum.PT;
		}

		public Units(UnitEnum value)
		{
			EnumValue = value;
		}

		public Units(string idString)
		{
			UnitEnum temp;
			if (Enum.TryParse<UnitEnum>(idString, true, out temp))
			{
				EnumValue = temp;
			}
			else
			{
				EnumValue = UnitEnum.PT;
			}
		}

		#region 公开方法

		public static bool IsIdValid(string idString)
		{
			switch (idString)
			{
				case "pt":
				case "in":
				case "mm":
				case "cm":
				case "pc":
					return true;
				default:
					return false;
			}
		}

		// TODO: *需要做本地化
		public string ToName()
		{
			switch (EnumValue)
			{
				case UnitEnum.PT:
					return "points";
				case UnitEnum.IN:
					return "inches";
				case UnitEnum.MM:
					return "mm";
				case UnitEnum.CM:
					return "cm";
				case UnitEnum.PC:
					return "picas";
				default:
					return string.Empty;
			}
		}

		public float Resolution()
		{
			switch (EnumValue)
			{
				case UnitEnum.PT:
					return 0.01f;
				case UnitEnum.IN:
					return 0.001f;
				case UnitEnum.MM:
					return 0.01f;
				case UnitEnum.CM:
					return 0.001f;
				case UnitEnum.PC:
					return 0.01f;
				default:
					return 0;
			}
		}

		public int ResolutionDigits()
		{
			switch (EnumValue)
			{
				case UnitEnum.PT:
					return 2;
				case UnitEnum.IN:
					return 3;
				case UnitEnum.MM:
					return 2;
				case UnitEnum.CM:
					return 3;
				case UnitEnum.PC:
					return 2;
				default:
					return 0;
			}
		}

		public string ToIdString()
		{
			return EnumValue.ToString().ToLower();
		}

		#endregion
	}
}