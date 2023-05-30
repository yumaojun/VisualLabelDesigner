using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 分类
	/// </summary>
	public class XmlCategoryParser
	{
		public bool ReadFile(string fileName)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(fileName);

			XmlNode root = doc.DocumentElement;
			if (root.Name != "Glabels-categories")
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
				if (child.Name == "Category")
				{
					ParseCategoryNode(child);
				}
			}
		}

		private void ParseCategoryNode(XmlNode node)
		{
			string id = XmlUtil.GetStringAttr(node, "id", "");
			string name = XmlUtil.GetI18nAttr(node, "name", "");

			var category = new Category(id, name);
			if (category != null)
			{
				Db.RegisterCategory(category);
			}
		}
	}
}
