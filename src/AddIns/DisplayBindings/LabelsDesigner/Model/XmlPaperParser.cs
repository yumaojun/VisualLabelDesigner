using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace YProgramStudio.LabelsDesigner.Model
{
	public class XmlPaperParser
	{
		public bool ReadFile(string fileName)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(fileName);

			XmlElement root = doc.DocumentElement;
			if (root.Name != "Glabels-paper-sizes")
			{
				return false;
			}

			ParseRootNode(root);
			return true;
		}

		private void ParseRootNode(XmlNode node)
		{
			for (var child = node.FirstChild; child != null; child = child.NextSibling)
			{
				if (child.Name == "Paper-size")
				{
					ParsePaperSizeNode(child);
				}
			}
		}

		private void ParsePaperSizeNode(XmlNode node)
		{
			string id = XmlUtil.GetStringAttr(node, "id", "");
			string name = XmlUtil.GetI18nAttr(node, "name", "");

			Distance width = XmlUtil.GetLengthAttr(node, "width", new Distance(0));
			Distance height = XmlUtil.GetLengthAttr(node, "height", new Distance(0));

			string pwgSize = XmlUtil.GetStringAttr(node, "pwg_size", "");

			var paper = new Paper(id, name, width, height, pwgSize);
			if (paper != null)
			{
				Db.RegisterPaper(paper);
			}
		}
	}
}
