using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	public class Variables : Dictionary<string, Variable>
	{
		public Variables() { }

		public Variables(Variables variables)
		{
			foreach (var key in variables.Keys)
			{
				Add(key, variables[key]);
			}
		}

		public Variables Clone()
		{
			return new Variables(this);
		}

		public void ResetVariables()
		{

		}

		public void IncrementVariablesOnItem()
		{

		}

		public void IncrementVariablesOnCopy()
		{

		}

		public void IncrementVariablesOnPage()
		{

		}
	}
}
