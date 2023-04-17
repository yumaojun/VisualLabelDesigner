using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace YProgramStudio.ZPLTextEditor.Services
{
	public class ComboBoxParameter : BaseZPLParameter
	{
		[XmlArray("ComboBoxValues")]
		[XmlArrayItem("ComboBoxValue", typeof(ComboBoxValue))]
		public List<ComboBoxValue> Values = new List<ComboBoxValue>();
	}

	public class TextBoxParameter : BaseZPLParameter
	{
		[XmlAttribute(AttributeName = "Size")]
		public int Size { get; set; }
	}

	public class ButtonParameter : BaseZPLParameter
	{
	}

	public class NumericBoxParameter : BaseZPLParameter
	{
		[XmlAttribute(AttributeName = "Min")]
		public int Min { get; set; }

		[XmlAttribute(AttributeName = "Max")]
		public int Max { get; set; }

		[XmlAttribute(AttributeName = "Size")]
		public int Size { get; set; }

		[XmlAttribute(AttributeName = "Increment")]
		public decimal Increment { get; set; }
	}

	[XmlRoot("ZPLCommandParameter")]
	public class ZPLCommandParameter
	{
		[XmlElement(typeof(ComboBoxParameter), ElementName = "ComboBox")]
		[XmlElement(typeof(TextBoxParameter), ElementName = "TextBox")]
		[XmlElement(typeof(ButtonParameter), ElementName = "Button")]
		[XmlElement(typeof(NumericBoxParameter), ElementName = "NumericBox")]
		public List<BaseZPLParameter> ZPLCommandParameters { get; set; }
	}
}
