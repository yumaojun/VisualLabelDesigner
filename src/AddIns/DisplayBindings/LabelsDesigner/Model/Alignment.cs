using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 对齐方式
	/// </summary>
	public enum Alignment
	{
		Left = 0x0001,
		Right = 0x0002,
		HCenter = 0x0004, // horizontal center

		Top = 0x0020,
		Bottom = 0x0040,
		VCenter = 0x0080, // vertical center

		Center = VCenter | HCenter
	}
}
