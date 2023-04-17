using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace YProgramStudio.ZPLTextEditor.Services
{
	public abstract class BaseZPLParameter
	{
		[XmlAttribute(AttributeName = "Name")]
		public string Name { get; set; }

		[XmlAttribute(AttributeName = "Code")]
		public string Code { get; set; }

		[XmlAttribute(AttributeName = "Description")]
		public string Description { get; set; }

		[XmlAttribute(AttributeName = "AcceptedValue")]
		public string AcceptedValue { get; set; }

		[XmlAttribute(AttributeName = "Default")]
		public string Default { get; set; }
	}
}
