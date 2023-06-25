using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Labels
{
	/// <summary>
	/// 通用帮助类
	/// </summary>
	public class CommonHelper
	{
		/// <summary>
		/// 获取设备物理DPI
		/// </summary>
		public static int PhysicalDpiX()
		{
			int physicalDpiX = 158; // TODO: *获取设备物理DPI
			return physicalDpiX;
		}

		public static int WindowDpiX()
		{
			int windowDpiX = 120; // TODO: *获取默认DPI
			return windowDpiX;
		}
	}
}
