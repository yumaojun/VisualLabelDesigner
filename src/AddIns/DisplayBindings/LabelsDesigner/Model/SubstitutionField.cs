using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	public class SubstitutionField
	{
		private string _fieldName;
		private string _defaultValue;
		private string _format;
		private char _formatType;
		private bool _newLine;



		public string Evaluate(Backends.Merge.Record record, Variables variables)
		{
			string value = _defaultValue;

			bool haveRecordField = record != null && record.ContainsKey(_fieldName) && record[_fieldName] != null;
			bool haveVariable = variables != null && variables.ContainsKey(_fieldName) && variables[_fieldName].Value() != null;

			if (haveRecordField)
			{
				value = record[_fieldName];
			}
			else if (haveVariable)
			{
				value = variables[_fieldName].Value();
			}

			if (!char.IsWhiteSpace(_formatType))
			{
				value = FormatValue(value);
			}

			if (_newLine && (haveRecordField || haveVariable))
			{
				value = "\n" + value;
			}

			return value;
		}

		private string FormatValue(string value)
		{
			switch (_formatType)
			{
				case 'd':
				case 'i':
					//return QString::asprintf(mFormat.toStdString().c_str(),
					//						  value.toLongLong(nullptr, 0));
					return string.Format("{0}", value);

				case 'u':
				case 'x':
				case 'X':
				case 'o':
					//return QString::asprintf(mFormat.toStdString().c_str(),
					//						  value.toULongLong(nullptr, 0));
					return string.Format("{0}", value);

				case 'f':
				case 'F':
				case 'e':
				case 'E':
				case 'g':
				case 'G':
					//return QString::asprintf(mFormat.toStdString().c_str(),
					//						  value.toDouble());
					return string.Format("{0}", value);

				case 's':
					//return QString::asprintf(mFormat.toStdString().c_str(),
					//						  value.toStdString().c_str());
					return string.Format("{0}", value);

				default:
					// Invalid format
					return string.Empty;
			}
		}

		// TODO：Parse(待优化
		public static bool Parse(string s, SubstitutionField field)
		{
			string sTmp = s;

			if (sTmp.StartsWith("${"))
			{
				sTmp = sTmp.Substring(2);

				if (ParseFieldName(sTmp, field))
				{
					while (sTmp.Length > 0 && sTmp[0] == ':')
					{
						sTmp = sTmp.Substring(1);
						if (!ParseModifier(sTmp, field))
						{
							return false;
						}
					}

					if (sTmp.Length > 0 && sTmp[0] == '}')
					{
						sTmp = sTmp.Substring(1);

						// Success.  Update s.
						s = sTmp;
						return true;
					}
				}
			}

			return false;
		}

		private static bool ParseFieldName(string s, SubstitutionField field)
		{
			return false;
		}

		private static bool ParseModifier(string s, SubstitutionField field)
		{
			return false;
		}

		private static bool ParseDefaultValueModifier(string s, SubstitutionField field)
		{
			return false;
		}

		private static bool ParseFormatModifier(string s, SubstitutionField field)
		{
			return false;
		}

		private static bool ParseFormatFlags(string s, SubstitutionField field)
		{
			return false;
		}

		private static bool ParseFormatWidth(string s, SubstitutionField field)
		{
			return false;
		}

		private static bool ParseFormatPrecision(string s, SubstitutionField field)
		{
			return false;
		}

		private static bool ParseFormatType(string s, SubstitutionField field)
		{
			return false;
		}

		private static bool ParseNaturalInteger(string s, SubstitutionField field)
		{
			return false;
		}

		private static bool ParseNewLineModifier(string s, SubstitutionField field)
		{
			return false;
		}

	}
}
