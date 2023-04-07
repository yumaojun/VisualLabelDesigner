using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VisualLabelDesigner.ZplTextEditor.Services
{
	public class ComboBoxValue
	{
		[XmlAttribute(AttributeName = "Value")]
		public string Value { get; set; }

		[XmlText]
		public string Content { get; set; }
	}
}
