using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace YProgramStudio.LabelsDesigner.Model
{
	public static class XmlLabelParser_3
	{
		public static Model ParseRootNode(XmlNode node)
		{
			// TODO: * Code for version 3 这是为向前兼容
			return new Model();
		}
	}
}
