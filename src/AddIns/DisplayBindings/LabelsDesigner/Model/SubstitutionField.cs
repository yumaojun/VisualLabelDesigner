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
		public static bool Parse(ref string s, SubstitutionField field)
		{
			string sTmp = s;

			if (sTmp.StartsWith("${"))
			{
				sTmp = sTmp.Substring(2);

				if (ParseFieldName(ref sTmp, field))
				{
					while (sTmp.Length > 0 && sTmp[0] == ':')
					{
						sTmp = sTmp.Substring(1);
						if (!ParseModifier(ref sTmp, field))
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

		private static bool ParseFieldName(ref string s, SubstitutionField field)
		{
			bool success = false;

			while (s.Length > 0 && char.IsLetterOrDigit(s[0]) && s[0] != ':' && s[0] != '}') // todo: 需要进一步验证是否等价于c.isPrint()
			{
				field._fieldName.Append(s[0]);
				s = s.Substring(1);

				success = true;
			}

			return success;
		}

		private static bool ParseModifier(ref string s, SubstitutionField field)
		{
			bool success = false;

			if (s.Length > 0 && s[0] == '%')
			{
				s = s.Substring(1);
				success = ParseFormatModifier(ref s, field);
			}
			else if (s.Length > 0 && s[0] == '=')
			{
				s = s.Substring(1);
				success = ParseDefaultValueModifier(ref s, field);
			}
			else if (s.Length > 0 && s[0] == 'n')
			{
				s = s.Substring(1);
				success = ParseNewLineModifier(ref s, field);
			}

			return success;
		}

		private static bool ParseDefaultValueModifier(ref string s, SubstitutionField field)
		{
			field._defaultValue = string.Empty;

			while (s.Length > 0 && !":}".Contains(s[0]))
			{
				if (s[0] == '\\')
				{
					s = s.Substring(1); // Skip escape
					if (s.Length > 0)
					{
						field._defaultValue.Append(s[0]);
						s = s.Substring(1);
					}
				}
				else
				{
					field._defaultValue.Append(s[0]);
					s = s.Substring(1);
				}
			}

			return true;
		}

		private static bool ParseFormatModifier(ref string s, SubstitutionField field)
		{
			field._format = "%";

			ParseFormatFlags(ref s, field);
			ParseFormatWidth(ref s, field);

			if (s.Length > 0 && s[0] == '.')
			{
				field._format += ".";
				s = s.Substring(1);
				ParseFormatPrecision(ref s, field);
			}

			ParseFormatType(ref s, field);

			return true; // Don't let invalid formats kill entire SubstitutionField
		}

		private static bool ParseFormatFlags(ref string s, SubstitutionField field)
		{
			while (s.Length > 0 && "-+ 0".Contains(s[0]))
			{
				field._format += s[0];
				s = s.Substring(1);
			}

			return true;
		}

		private static bool ParseFormatWidth(ref string s, SubstitutionField field)
		{
			return ParseNaturalInteger(ref s, field);
		}

		private static bool ParseFormatPrecision(ref string s, SubstitutionField field)
		{
			return ParseNaturalInteger(ref s, field);
		}

		private static bool ParseFormatType(ref string s, SubstitutionField field)
		{
			bool success = false;

			if (s.Length > 0 && "diufFeEgGxXos".Contains(s[0]))
			{
				field._formatType = s[0];
				field._format += s[0];
				s = s.Substring(1);
				success = true;
			}

			return success;
		}

		private static bool ParseNaturalInteger(ref string s, SubstitutionField field)
		{
			bool success = false;

			if (s.Length > 0 && s[0] >= '1' && s[0] <= '9')
			{
				field._format += s[0];
				s = s.Substring(1);

				while (s.Length > 0 && char.IsDigit(s[0]))
				{
					field._format += s[0];
					s = s.Substring(1);
				}

				success = true;
			}

			return success;
		}

		private static bool ParseNewLineModifier(ref string s, SubstitutionField field)
		{
			field._newLine = true;
			return true;
		}
	}
}
