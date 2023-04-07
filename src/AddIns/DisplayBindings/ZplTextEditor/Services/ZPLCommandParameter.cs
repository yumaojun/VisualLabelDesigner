using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VisualLabelDesigner.ZplTextEditor.Services
{
	public class ComboBoxParameter : BaseZplParameter
	{
		[XmlArray("ComboBoxValues")]
		[XmlArrayItem("ComboBoxValue", typeof(ComboBoxValue))]
		public List<ComboBoxValue> Values = new List<ComboBoxValue>();
	}

	public class TextBoxParameter : BaseZplParameter
	{
		[XmlAttribute(AttributeName = "Size")]
		public int Size { get; set; }
	}

	public class ButtonParameter : BaseZplParameter
	{
	}

	public class NumericBoxParameter : BaseZplParameter
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
		public List<BaseZplParameter> ZPLCommandParameters { get; set; }
	}
}
