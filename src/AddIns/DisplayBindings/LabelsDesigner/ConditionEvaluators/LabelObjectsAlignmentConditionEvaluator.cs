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
	/// 选中了多个对象
	/// </summary>
	public class LabelObjectsAlignmentConditionEvaluator : IConditionEvaluator
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

				return lvc.IsMultiSelected;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
