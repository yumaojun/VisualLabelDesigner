using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 供应商
	/// </summary>
	public class XmlVendorParser
	{
		public bool ReadFile(string fileName)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(fileName);

			XmlNode root = doc.DocumentElement;
			if (root.Name != "Glabels-vendors")
			{
				return false;
			}

			ParseRootNode(root);
			return true;
		}

		private void ParseRootNode(XmlNode node)
		{
			for (XmlNode child = node.FirstChild; child != null; child = child.NextSibling)
			{
				if (child.Name == "Vendor")
				{
					ParseVendorNode(child);
				}
			}
		}

		private void ParseVendorNode(XmlNode node)
		{
			string name = XmlUtil.GetStringAttr(node, "name", "");
			string url = XmlUtil.GetStringAttr(node, "url", "");

			var vendor = new Vendor(name, url);
			if (vendor != null)
			{
				Db.RegisterVendor(vendor);
			}
		}
	}
}
