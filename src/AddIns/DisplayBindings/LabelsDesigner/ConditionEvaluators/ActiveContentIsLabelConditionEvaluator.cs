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
	/// 判断是否当前文件类型、和选中的元素对象
	/// </summary>
	public class ActiveContentIsLabelConditionEvaluator : IConditionEvaluator
	{
		public bool IsValid(object parameter, Condition condition)
		{
			if (SD.Workbench == null || SD.Workbench.ActiveWorkbenchWindow == null || SD.Workbench.ActiveViewContent == null)
			{
				return false;
			}

			try
			{
				//string name = SD.Workbench.ActiveViewContent.PrimaryFileName;

				//if (name == null)
				//{
				//	return false;
				//}

				//string extension = Path.GetExtension(name);
				//return extension.ToUpperInvariant() == condition.Properties["activeextension"].ToUpperInvariant();
				var lvc = SD.Workbench.ActiveViewContent as LabelViewContent;

				if (lvc == null)
				{
					return false;
				}

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
