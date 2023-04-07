using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualLabelDesigner.ZplTextEditor.Services
{
	public class VariableSubstitution
	{
		public static Dictionary<string, string> Load()
		{
			Dictionary<string, string> result;
			try
			{
				string path = VarsFile.VariableSubstitution();
				if (File.Exists(path))
				{
					result = (from line in File.ReadLines(path)
							  select line.Split(new char[] { '\t' })).ToDictionary((string[] line) => line[0], (string[] line) => line[1]);
				}
				else
				{
					result = new Dictionary<string, string>();
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			return result;
		}
	}
}
