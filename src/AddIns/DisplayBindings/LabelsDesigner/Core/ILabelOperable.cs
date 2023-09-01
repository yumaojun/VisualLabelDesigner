using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Core
{
	/// <summary>
	/// 标签有对象选中可操作
	/// </summary>
	interface ILabelOperable
	{
		/// <summary>
		/// 有任何一个选中
		/// </summary>
		bool IsAnySelected { get; }

		/// <summary>
		/// 有两个以上选中
		/// </summary>
		bool IsMultiSelected { get; }

	}
}
