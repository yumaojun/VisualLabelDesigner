using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 处理模板xml
	/// </summary>
	public class XmlTemplateParser
	{
		#region 公开方法

		public bool ReadFile(string fileName, bool isUserDefined)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(fileName);

			var root = doc.DocumentElement;
			if (root.Name != "Glabels-templates")
			{
				return false;
			}
#if DEBUG
			if (fileName.EndsWith("rayfilm-templates.xml"))
			{

			}
#endif

			ParseRootNode(root, isUserDefined);
			return true;
		}

		public Template ParseTemplateNode(XmlNode node, bool isUserDefined = false)
		{
			string brand = node.Attributes["brand"].Value ?? string.Empty;
			string part = node.Attributes["part"].Value ?? string.Empty;

#if DEBUG
			if (part == "ABP-237")
			{

			}
#endif

			if (brand == string.Empty || part == string.Empty)
			{
				string name = node.Attributes["name"].Value ?? string.Empty;
				if (name != string.Empty)
				{
					var fields = name.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
					brand = fields[0];
					part = fields[1];
				}
			}

			Template template = null;

			string equivPart = node.Attributes["equiv"]?.Value ?? string.Empty;

			if (equivPart != string.Empty)
			{
				template = Template.FromEquiv(brand, part, equivPart);

				for (XmlNode child = node.FirstChild; child != null; child = child.NextSibling)
				{
					if (child.Name == "Meta")
					{
						ParseMetaNode(child, template);
					}
				}
			}
			else
			{
				string description = XmlUtil.GetI18nAttr(node, "description", "");
				string paperId = XmlUtil.GetStringAttr(node, "size", "");

				if (Db.IsPaperIdKnown(paperId))
				{
					Paper paper = Db.LookupPaperFromId(paperId);
					if (paper == null)
					{
						return null;
					}

					template = new Template(brand, part, description, paper.Id, paper.Width, paper.Height, 0F, isUserDefined);
				}
				else
				{
					Distance width = XmlUtil.GetLengthAttr(node, "width", new Distance(0));
					Distance height = XmlUtil.GetLengthAttr(node, "height", new Distance(0));
					Distance rollWidth = XmlUtil.GetLengthAttr(node, "roll_width", new Distance(0));

					template = new Template(brand, part, description, paperId, width, height, rollWidth, isUserDefined);
				}

				for (XmlNode child = node.FirstChild; child != null; child = child.NextSibling)
				{
					if (child.Name == "Meta")
					{
						ParseMetaNode(child, template);
					}
					else if (child.Name == "Label-rectangle")
					{
						ParseLabelRectangleNode(child, template);
					}
					else if (child.Name == "Label-ellipse")
					{
						ParseLabelEllipseNode(child, template);
					}
					else if (child.Name == "Label-round")
					{
						ParseLabelRoundNode(child, template);
					}
					else if (child.Name == "Label-cd")
					{
						ParseLabelCdNode(child, template);
					}
					else if (child.Name == "Label-path")
					{
						ParseLabelPathNode(child, template);
					}
					else if (child.Name == "Label-continuous")
					{
						ParseLabelContinuousNode(child, template);
					}
				}
			}

			return template;
		}

		#endregion

		#region 内部方法

		private void ParseRootNode(XmlElement root, bool isUserDefined)
		{
			for (XmlNode child = root.FirstChild; child != null; child = child.NextSibling)
			{
				if (child?.Name == "Template")
				{
					Template tmplate = ParseTemplateNode(child, isUserDefined);
					if (tmplate != null)
					{
						Db.RegisterTemplate(tmplate);
					}
				}
			}
		}

		private void ParseMetaNode(XmlNode node, Template template)
		{
			string productUrl = node.Attributes["product_url"]?.Value ?? string.Empty;
			if (productUrl != string.Empty)
			{
				template.ProductUrl = productUrl;
			}

			string categoryId = node.Attributes["category"]?.Value ?? string.Empty;
			if (categoryId != string.Empty)
			{
				template.AddCategory(categoryId);
			}
		}

		private void ParseLabelRectangleNode(XmlNode node, Template tmplate)
		{
			string id = XmlUtil.GetStringAttr(node, "id", "0");
			Distance w = XmlUtil.GetLengthAttr(node, "width",new Distance(0));
			Distance h = XmlUtil.GetLengthAttr(node, "height", new Distance(0));
			Distance r = XmlUtil.GetLengthAttr(node, "round", new Distance(0));

			Distance xWaste, yWaste;
			Distance waste = XmlUtil.GetLengthAttr(node, "waste", new Distance(-1));
			if (waste >= new Distance(0))
			{
				xWaste = waste;
				yWaste = waste;
			}
			else
			{
				xWaste = XmlUtil.GetLengthAttr(node, "x_waste", new Distance(0));
				yWaste = XmlUtil.GetLengthAttr(node, "y_waste", new Distance(0));
			}

			Frame frame = new FrameRect(w, h, r, xWaste, yWaste, id);
			ParseLabelNodeCommon(node, frame);
			tmplate.AddFrame(frame);
		}

		private void ParseLabelEllipseNode(XmlNode node, Template tmplate)
		{
			string id = XmlUtil.GetStringAttr(node, "id", "0");
			Distance w = XmlUtil.GetLengthAttr(node, "width", new Distance(0));
			Distance h = XmlUtil.GetLengthAttr(node, "height", new Distance(0));
			Distance waste = XmlUtil.GetLengthAttr(node, "waste", new Distance(0));

			Frame frame = new FrameEllipse(w, h, waste, id);
			ParseLabelNodeCommon(node, frame);
			tmplate.AddFrame(frame);
		}

		private void ParseLabelRoundNode(XmlNode node, Template tmplate)
		{
			string id = XmlUtil.GetStringAttr(node, "id", "0");
			Distance r = XmlUtil.GetLengthAttr(node, "radius",new Distance(0));
			Distance waste = XmlUtil.GetLengthAttr(node, "waste", new Distance(0));

			Frame frame = new FrameRound(r, waste, id);
			ParseLabelNodeCommon(node, frame);
			tmplate.AddFrame(frame);
		}

		private void ParseLabelCdNode(XmlNode node, Template tmplate)
		{
			string id = XmlUtil.GetStringAttr(node, "id", "0");
			Distance r1 = XmlUtil.GetLengthAttr(node, "radius", new Distance(0));
			Distance r2 = XmlUtil.GetLengthAttr(node, "hole", new Distance(0));
			Distance w = XmlUtil.GetLengthAttr(node, "width", new Distance(0));
			Distance h = XmlUtil.GetLengthAttr(node, "height", new Distance(0));
			Distance waste = XmlUtil.GetLengthAttr(node, "waste", new Distance(0));

			Frame frame = new FrameCd(r1, r2, w, h, waste, id);
			ParseLabelNodeCommon(node, frame);
			tmplate.AddFrame(frame);
		}

		private void ParseLabelPathNode(XmlNode node, Template tmplate)
		{
			string id = XmlUtil.GetStringAttr(node, "id", "0");
			Units dUnits = XmlUtil.GetUnitsAttr(node, "d_units", Units.Pc);
			var d = XmlUtil.GetPathDataAttr(node, "d", dUnits);

			Distance xWaste, yWaste;

			Distance waste = XmlUtil.GetLengthAttr(node, "waste",new  Distance(-1));
			if (waste >= new Distance(0))
			{
				xWaste = waste;
				yWaste = waste;
			}
			else
			{
				xWaste = XmlUtil.GetLengthAttr(node, "x_waste", new Distance(0));
				yWaste = XmlUtil.GetLengthAttr(node, "y_waste", new Distance(0));
			}

			Frame frame = new FramePath(d, xWaste, yWaste, dUnits, id);
			ParseLabelNodeCommon(node, frame);
			tmplate.AddFrame(frame);
		}

		private void ParseLabelContinuousNode(XmlNode node, Template tmplate)
		{
			string id = XmlUtil.GetStringAttr(node, "id", "0");
			Distance w = XmlUtil.GetLengthAttr(node, "width",new Distance(0));
			Distance h = XmlUtil.GetLengthAttr(node, "height", new Distance(0));
			Distance hMin = XmlUtil.GetLengthAttr(node, "min_height", new Distance(0));
			Distance hMax = XmlUtil.GetLengthAttr(node, "max_height", new Distance(0));
			Distance hDefault = XmlUtil.GetLengthAttr(node, "default_height", new Distance(0));

			Frame frame = new FrameContinuous(w, hMin, hMax, hDefault, id);
			if (h > new Distance(0))
			{
				frame.Height = h;
			}

			ParseLabelNodeCommon(node, frame);
			tmplate.AddFrame(frame);
		}

		private void ParseLabelNodeCommon(XmlNode node, Frame frame)
		{
			for (XmlNode child = node.FirstChild; child != null; child = child.NextSibling)
			{
				if (child.Name == "Layout")
				{
					ParseLayoutNode(child, frame);
				}
				else if (child.Name == "Markup-margin")
				{
					ParseMarkupMarginNode(child, frame);
				}
				else if (child.Name == "Markup-line")
				{
					ParseMarkupLineNode(child, frame);
				}
				else if (child.Name == "Markup-circle")
				{
					ParseMarkupCircleNode(child, frame);
				}
				else if (child.Name == "Markup-rect")
				{
					ParseMarkupRectNode(child, frame);
				}
				else if (child.Name == "Markup-ellipse")
				{
					ParseMarkupEllipseNode(child, frame);
				}
			}
		}

		private void ParseLayoutNode(XmlNode node, Frame frame)
		{
			int nX = XmlUtil.GetIntAttr(node, "nx", 1);
			int nY = XmlUtil.GetIntAttr(node, "ny", 1);
			Distance x0 = XmlUtil.GetLengthAttr(node, "x0", new Distance(0));
			Distance y0 = XmlUtil.GetLengthAttr(node, "y0", new Distance(0));
			Distance dX = XmlUtil.GetLengthAttr(node, "dx", new Distance(0));
			Distance dY = XmlUtil.GetLengthAttr(node, "dy", new Distance(0));
			frame.AddLayout(new Layout(nX, nY, x0, y0, dX, dY));
		}

		private void ParseMarkupMarginNode(XmlNode node, Frame frame)
		{
			Distance size = XmlUtil.GetLengthAttr(node, "size", new Distance(0));
			Distance xSize = XmlUtil.GetLengthAttr(node, "x_size", new Distance(0));
			Distance ySize = XmlUtil.GetLengthAttr(node, "y_size", new Distance(0));

			if (size > new Distance(0))
			{
				frame.AddMarkup(new MarkupMargin(size));
			}
			else
			{
				frame.AddMarkup(new MarkupMargin(xSize, ySize));
			}
		}

		private void ParseMarkupLineNode(XmlNode node, Frame frame)
		{
			Distance x1 = XmlUtil.GetLengthAttr(node, "x1", new Distance(0));
			Distance y1 = XmlUtil.GetLengthAttr(node, "y1",new Distance(0));
			Distance x2 = XmlUtil.GetLengthAttr(node, "x2",new Distance(0));
			Distance y2 = XmlUtil.GetLengthAttr(node, "y2",new Distance(0));
			frame.AddMarkup(new MarkupLine(x1, y1, x2, y2));
		}

		private void ParseMarkupCircleNode(XmlNode node, Frame frame)
		{
			Distance x0 = XmlUtil.GetLengthAttr(node, "x0", new Distance(0));
			Distance y0 = XmlUtil.GetLengthAttr(node, "y0", new Distance(0));
			Distance r = XmlUtil.GetLengthAttr(node, "radius", new Distance(0));
			frame.AddMarkup(new MarkupCircle(x0, y0, r));
		}

		private void ParseMarkupRectNode(XmlNode node, Frame frame)
		{
			Distance x1 = XmlUtil.GetLengthAttr(node, "x1", new Distance(0));
			Distance y1 = XmlUtil.GetLengthAttr(node, "y1", new Distance(0));
			Distance w = XmlUtil.GetLengthAttr(node, "w", new Distance(0));
			Distance h = XmlUtil.GetLengthAttr(node, "h", new Distance(0));
			Distance r = XmlUtil.GetLengthAttr(node, "r", new Distance(0));
			frame.AddMarkup(new MarkupRect(x1, y1, w, h, r));
		}

		private void ParseMarkupEllipseNode(XmlNode node, Frame frame)
		{
			Distance x1 = XmlUtil.GetLengthAttr(node, "x1", new Distance(0));
			Distance y1 = XmlUtil.GetLengthAttr(node, "y1", new Distance(0));
			Distance w = XmlUtil.GetLengthAttr(node, "w", new Distance(0));
			Distance h = XmlUtil.GetLengthAttr(node, "h", new Distance(0));
			frame.AddMarkup(new MarkupEllipse(x1, y1, w, h));
		}

		#endregion
	}
}
