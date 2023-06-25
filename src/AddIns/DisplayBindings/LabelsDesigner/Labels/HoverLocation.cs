using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Labels
{
	/// <summary>
	/// 对象选取框边角位置枚举
	/// </summary>
	public enum HoverLocation
	{
		/// <summary>
		/// NorthWest
		/// </summary>
		NW,
		/// <summary>
		/// North
		/// </summary>
		N,
		/// <summary>
		/// NorthEast
		/// </summary>
		NE,
		/// <summary>
		/// East
		/// </summary>
		E,
		/// <summary>
		/// SouthEast
		/// </summary>
		SE,
		/// <summary>
		/// South
		/// </summary>
		S,
		/// <summary>
		/// SouthWest
		/// </summary>
		SW,
		/// <summary>
		/// West
		/// </summary>
		W,
		/// <summary>
		/// 直线起始点
		/// </summary>
		P1,
		/// <summary>
		/// 直线结束点
		/// </summary>
		P2,
		/// <summary>
		/// 旋转
		/// </summary>
		Rotate
	};
}
