using ICSharpCode.Core;
using ICSharpCode.SharpDevelop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.ConditionEvaluators
{
	/// <summary>
	/// 选中了对象
	/// </summary>
	public class LabelObjectsSelectConditionEvaluator : IConditionEvaluator
	{
		public bool IsValid(object parameter, Condition condition)
		{
			if (SD.Workbench == null || SD.Workbench.ActiveWorkbenchWindow == null || SD.Workbench.ActiveViewContent == null)
			{
				return false;
			}

			try
			{
				var lvc = SD.Workbench.ActiveViewContent as LabelViewContent;

				if (lvc == null)
				{
					return false;
				}

				return lvc.IsAnySelected;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
