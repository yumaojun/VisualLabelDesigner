using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 常量
	/// </summary>
	public class Constants
	{
		public const float PTS_PER_PT = 1.0F;
		public const float PTS_PER_INCH = 72.0F;
		public const float PTS_PER_MM = 2.83464566929F;
		public const float PTS_PER_CM = (10.0F * PTS_PER_MM);
		public const float PTS_PER_PICA = 12.0F;
		public static readonly Distance EPSILON = new Distance(0.5F, UnitEnum.PT);
	}
}
