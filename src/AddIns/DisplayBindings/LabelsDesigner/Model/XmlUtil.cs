using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace YProgramStudio.LabelsDesigner.Model
{
	public enum StateEnum { CMD, MX, MY, MDX, MDY, LX, LY, LDX, LDY, HX, HDX, VY, VDY }

	/// <summary>
	/// Xml工具类
	/// </summary>
	public class XmlUtil
	{
		#region 公开方法

		public static string GetStringAttr(XmlNode node, string name, string defaultValue = "")
		{
			return node.Attributes[name]?.Value ?? defaultValue;
		}

		public static string GetI18nAttr(XmlNode node, string name, string defaultValue = "")
		{
			string i18nString = node.Attributes["_" + name]?.Value ?? defaultValue;

			if (i18nString == string.Empty)
			{
				return node.Attributes[name]?.Value ?? defaultValue;
			}

			return i18nString; // Here need to convert as i18nString
		}

		public static Distance GetLengthAttr(XmlNode node, string name, Distance defaultValue)
		{
			string valueString = node.Attributes[name]?.Value ?? string.Empty;
			if (valueString != string.Empty)
			{
				var value = 0f;
				float.TryParse(Regex.Replace(valueString, @"[^\d.\d]", ""), out value);
				string unitsString = Regex.Replace(valueString, @"[\d.\d]", "");
				if (unitsString != string.Empty && !Units.IsIdValid(unitsString))
				{
					return defaultValue;
				}
				return new Distance(value, unitsString);
			}
			return defaultValue;
		}

		public static float GetFloatAttr(XmlNode node, string name, float defaultValue)
		{
			string valueString = node.Attributes[name]?.Value ?? string.Empty;
			if (valueString != string.Empty)
			{
				float value;
				if (float.TryParse(valueString, out value))
				{
					return value;
				}
			}
			return defaultValue;
		}

		public static bool GetBoolAttr(XmlNode node, string name, bool defaultValue)
		{
			string valueString = node.Attributes[name]?.Value ?? string.Empty;
			if (valueString != string.Empty)
			{
				bool value;
				int intValue;
				if (bool.TryParse(valueString, out value))
				{
					return value;
				}
				else if (int.TryParse(valueString, out intValue))
				{
					return intValue == 1;
				}
			}
			return defaultValue;
		}

		public static int GetIntAttr(XmlNode node, string name, int defaultValue)
		{
			string valueString = node.Attributes[name]?.Value ?? string.Empty;
			if (valueString != string.Empty)
			{
				int value;
				if (int.TryParse(valueString, out value))
				{
					return value;
				}
			}
			return defaultValue;
		}

		public static uint GetUIntAttr(XmlNode node, string name, uint default_value)
		{
			string valueString = node.Attributes[name]?.Value ?? string.Empty;
			if (valueString != string.Empty)
			{
				try
				{
					if (valueString.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
					{
						valueString = valueString.Remove(0, 2);
					}
					uint value = uint.Parse(valueString, System.Globalization.NumberStyles.AllowHexSpecifier);
					return value;
				}
				catch (Exception)
				{
				}
			}
			return default_value;
		}

		public static Units GetUnitsAttr(XmlNode node, string name, Units defaultValue)
		{
			string valueString = node.Attributes[name]?.Value ?? string.Empty;
			if (valueString != string.Empty)
			{
				return new Units(valueString);
			}
			return defaultValue;
		}

		public static SKFontStyleWeight GetWeightAttr(XmlNode node, string name, SKFontStyleWeight defaultValue)
		{
			string valueString = node.Attributes[name]?.Value ?? string.Empty;
			if (valueString != string.Empty)
			{
				if (valueString == "bold")
				{
					return SKFontStyleWeight.Bold;
				}
				else if (valueString == "normal")
				{
					return SKFontStyleWeight.Normal;
				}
			}
			return defaultValue;
		}

		public static Alignment GetAlignmentAttr(XmlNode node, string name, Alignment defaultValue)
		{
			string valueString = node.Attributes[name]?.Value ?? string.Empty;
			if (valueString != string.Empty)
			{
				if (valueString == "right")
				{
					return Alignment.Right;
				}
				else if (valueString == "hcenter")
				{
					return Alignment.HCenter;
				}
				else if (valueString == "left")
				{
					return Alignment.Left;
				}
				else if (valueString == "bottom")
				{
					return Alignment.Bottom;
				}
				else if (valueString == "vcenter")
				{
					return Alignment.VCenter;
				}
				else if (valueString == "top")
				{
					return Alignment.Top;
				}
			}
			return defaultValue;
		}

		public static WrapMode GetWrapModeAttr(XmlNode node, string name, WrapMode defaultValue)
		{
			string valueString = node.Attributes[name]?.Value ?? string.Empty;
			if (valueString != string.Empty)
			{
				if (valueString == "word")
				{
					return WrapMode.WordWrap;
				}
				else if (valueString == "anywhere")
				{
					return WrapMode.WrapAnywhere;
				}
				else if (valueString == "none")
				{
					return WrapMode.NoWrap;
				}

			}
			return defaultValue;
		}

		public static SkiaSharp.SKPath GetPathDataAttr(XmlNode node, string name, Units units)
		{
			var d = new SkiaSharp.SKPath();

			//
			// Simple path data parser
			//
			string[] tokens = (node.Attributes[name]?.Value ?? string.Empty).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

			StateEnum state = StateEnum.CMD;

			Distance x = 0;
			Distance y = 0;
			Distance dx = 0;
			Distance dy = 0;
			SkiaSharp.SKPoint c;

			for (int i = 0; i < tokens.Length; i++)
			{
				switch (state)
				{
					case StateEnum.CMD:
						switch (tokens[i][0])
						{
							case 'M':
								state = StateEnum.MX;
								break;
							case 'm':
								state = StateEnum.MDX;
								break;
							case 'L':
								state = StateEnum.LX;
								break;
							case 'l':
								state = StateEnum.LDX;
								break;
							case 'H':
								state = StateEnum.HX;
								break;
							case 'h':
								state = StateEnum.HDX;
								break;
							case 'V':
								state = StateEnum.VY;
								break;
							case 'v':
								state = StateEnum.VDY;
								break;
							case 'Z':
							case 'z':
								d.Close();
								state = StateEnum.CMD;
								break;
						}
						break;
					case StateEnum.MX:
						x = new Distance(float.Parse(tokens[i]), units);
						state = StateEnum.MY;
						break;
					case StateEnum.MY:
						y = new Distance(float.Parse(tokens[i]), units);
						d.MoveTo(x.Pt(), y.Pt());
						state = StateEnum.CMD;
						break;
					case StateEnum.MDX:
						dx = new Distance(float.Parse(tokens[i]), units);
						state = StateEnum.MDY;
						break;
					case StateEnum.MDY:
						dy = new Distance(float.Parse(tokens[i]), units);
						c = d.LastPoint;
						d.MoveTo(c.X + x.Pt(), c.Y + y.Pt());
						state = StateEnum.CMD;
						break;
					case StateEnum.LX:
						x = new Distance(float.Parse(tokens[i]), units);
						state = StateEnum.LY;
						break;
					case StateEnum.LY:
						y = new Distance(float.Parse(tokens[i]), units);
						d.LineTo(x.Pt(), y.Pt());
						state = StateEnum.CMD;
						break;
					case StateEnum.LDX:
						dx = new Distance(float.Parse(tokens[i]), units);
						state = StateEnum.LDY;
						break;
					case StateEnum.LDY:
						dy = new Distance(float.Parse(tokens[i]), units);
						c = d.LastPoint;
						d.LineTo(c.X + dx.Pt(), c.Y + dy.Pt());
						state = StateEnum.CMD;
						break;
					case StateEnum.HX:
						x = new Distance(float.Parse(tokens[i]), units);
						c = d.LastPoint;
						d.LineTo(x.Pt(), c.Y);
						state = StateEnum.CMD;
						break;
					case StateEnum.HDX:
						dx = new Distance(float.Parse(tokens[i]), units);
						c = d.LastPoint;
						d.LineTo(c.X + dx.Pt(), c.Y);
						state = StateEnum.CMD;
						break;
					case StateEnum.VY:
						y = new Distance(float.Parse(tokens[i]), units);
						c = d.LastPoint;
						d.LineTo(c.X, y.Pt());
						state = StateEnum.CMD;
						break;
					case StateEnum.VDY:
						dy = new Distance(float.Parse(tokens[i]), units);
						c = d.LastPoint;
						d.LineTo(c.X, c.Y + dy.Pt());
						state = StateEnum.CMD;
						break;
				}
			}

			return d;
		}

		public static void SetStringAttr(XmlNode node, string name, string value)
		{

		}

		public static void SetDoubleAttr(XmlNode node, string name, double value)
		{

		}

		public static void SetBoolAttr(XmlNode node, string name, bool value)
		{

		}

		public static void SetIntAttr(XmlNode node, string name, int value)
		{

		}

		public static void SetUIntAttr(XmlNode node, string name, uint value)
		{

		}

		public static void SetLengthAttr(XmlNode node, string name, Distance value)
		{

		}

		public static void SetWeightAttr(XmlNode node, string name, FontStyle value)
		{
			switch (value)
			{
				case FontStyle.Bold:
					node.Attributes[name].Value = "bold";
					break;
				default:
					node.Attributes[name].Value = "normal";
					break;
			}
		}

		public static void SetAlignmentAttr(XmlNode node, string name, Alignment value)
		{
			node.Attributes[name].Value = value.ToString().ToLower();
		}

		public static void SetWrapModeAttr(XmlNode node, string name, WrapMode value)
		{
			switch (value)
			{
				case WrapMode.WordWrap:
					node.Attributes[name].Value = "word";
					break;
				case WrapMode.WrapAnywhere:
					node.Attributes[name].Value = "anywhere";
					break;
				case WrapMode.NoWrap:
				case WrapMode.ManualWrap:
					node.Attributes[name].Value = "none";
					break;
				default:
					node.Attributes[name].Value = "word";
					break;
			}
		}

		public static void SetUnitsAttr(XmlNode node, string name, Units value)
		{
			node.Attributes[name].Value = value.ToIdString();
		}

		//public static void SetPathDataAttr(XmlNode node, string name, QPainterPath value, Units units)
		//{
		//	System.Drawing.Drawing2D.GraphicsPath p = new System.Drawing.Drawing2D.GraphicsPath();
		//	foreach (var pt1 in p.PathData.Points)
		//	{
		//		pt1.X;
		//		pt1.Y;
		//	}

		//	string pathString;
		//	for (int i = 0; i < path.elementCount(); i++)
		//	{
		//		auto element = path.elementAt(i);

		//		// QPainterPath is natively in pts
		//		Distance x = Distance::pt(element.x);
		//		Distance y = Distance::pt(element.y);

		//		// Translate desired units for path data
		//		double xValue = x.inUnits(units);
		//		double yValue = y.inUnits(units);

		//		if (element.isMoveTo())
		//		{
		//			pathString.append(QString("M %1 %2").arg(xValue).arg(yValue));
		//		}
		//		else if (element.isLineTo())
		//		{
		//			pathString.append(QString("L %1 %2").arg(xValue).arg(yValue));
		//		}

		//		if (i < (path.elementCount() - 1))
		//		{
		//			pathString.append(" ");
		//		}
		//	}

		//	node.setAttribute(name, pathString);
		//}

		#endregion

	}
}
