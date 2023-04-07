using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VisualLabelDesigner.ZplTextEditor
{
	public class ZPLCode
	{
		public static string Clean(string code)
		{
			StringBuilder stringBuilder = new StringBuilder(code);
			stringBuilder.Replace("\r", string.Empty);
			stringBuilder.Replace("\n", string.Empty);
			stringBuilder.Replace("\t", string.Empty);
			stringBuilder.Replace("\f", string.Empty);
			return stringBuilder.ToString().Trim();
		}

		public static string CompliantWithZebra(string code)
		{
			code = Uri.EscapeDataString(code);
			code = code.Replace("~", "%7E");
			return code;
		}

		public static List<string> GetVariables(string zplcode, string startdelimiter, string enddelimiter)
		{
			List<string> list = new List<string>();
			foreach (object obj in Regex.Matches(zplcode, string.Format("{0}(.*?){1}", startdelimiter, enddelimiter)))
			{
				Match match = (Match)obj;
				if (!list.Contains(match.Groups[1].Value))
				{
					list.Add(match.Groups[1].Value);
				}
			}
			return list;
		}

		public static string ReplaceVariable(string zplcode, Dictionary<string, string> _dicVariableSubstitution, string start, string end)
		{
			StringBuilder stringBuilder = new StringBuilder(zplcode);
			foreach (string arg in ZPLCode.GetVariables(zplcode, start, end))
			{
				string text = string.Format("{0}{1}{2}", start, arg, end);
				string text2;
				_dicVariableSubstitution.TryGetValue(text, out text2);
				if (!string.IsNullOrEmpty(text2))
				{
					stringBuilder.Replace(text, text2);
				}
			}
			return stringBuilder.ToString();
		}
	}
}
