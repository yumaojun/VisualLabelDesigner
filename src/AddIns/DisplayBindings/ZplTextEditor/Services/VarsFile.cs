using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.ZPLTextEditor.Services
{
	public static class VarsFile
	{
		public static string ApplicationDirectory()
		{
			return Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\VisualLabelDesigner";
		}

		public static string VariableSubstitution()
		{
			return VarsFile.ApplicationDirectory() + "\\myVars.txt";
		}
	}
}
