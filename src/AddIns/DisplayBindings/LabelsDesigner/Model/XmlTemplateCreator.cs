using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace YProgramStudio.LabelsDesigner.Model
{
	public class XmlTemplateCreator
	{
		public XmlTemplateCreator() { }

		#region 公开方法

		public bool WriteTemplates(List<Template> tmplates, string fileName)
		{
			XmlDocument doc = new XmlDocument();
			var root = doc.CreateElement("Glabels-templates");
			doc.AppendChild(root);
			try
			{
				foreach (Template tmplate in tmplates)
				{
					CreateTemplateNode(root, tmplate);
				}
				doc.Save(fileName);
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		public bool WriteTemplate(Template tmplate, string fileName)
		{
			List<Template> tmplates = new List<Template>();
			tmplates.Add(tmplate);
			return WriteTemplates(tmplates, fileName);
		}

		public void CreateTemplateNode(XmlNode parent, Template tmplate)
		{
			var doc = parent.OwnerDocument;
			var node = doc.CreateElement("Template");
			parent.AppendChild(node);

			XmlUtil.SetStringAttr(node, "brand", tmplate.Brand);
			XmlUtil.SetStringAttr(node, "part", tmplate.Part);

			XmlUtil.SetStringAttr(node, "size", tmplate.PaperId);
			if (tmplate.IsSizeOther)
			{
				XmlUtil.SetLengthAttr(node, "width", tmplate.PageWidth);
				XmlUtil.SetLengthAttr(node, "height", tmplate.PageHeight);
			}
			if (tmplate.IsRoll)
			{
				XmlUtil.SetLengthAttr(node, "roll_width", tmplate.RollWidth);
			}

			XmlUtil.SetStringAttr(node, "description", tmplate.Description);

			if (!string.IsNullOrEmpty(tmplate.ProductUrl))
			{
				CreateMetaNode(node, "product_url", tmplate.ProductUrl);
			}

			foreach (Frame frame in tmplate.Frames)
			{
				CreateLabelNode(node, frame);
			}
		}

		#endregion

		#region 私有方法

		void CreateMetaNode(XmlNode parent, string attr, string value) { }
		void CreateLabelNode(XmlNode parent, Frame frame) { }
		void CreateLabelRectangleNode(XmlNode parent, FrameRect frame) { }
		void CreateLabelEllipseNode(XmlNode parent, FrameEllipse frame) { }
		void CreateLabelRoundNode(XmlNode parent, FrameRound frame) { }
		void CreateLabelCdNode(XmlNode parent, FrameCd frame) { }
		void CreateLabelPathNode(XmlNode parent, FramePath frame) { }
		void CreateLabelContinuousNode(XmlNode parent, FrameContinuous frame) { }
		void CreateLabelNodeCommon(XmlNode node, Frame frame) { }
		void CreateLayoutNode(XmlNode parent, Layout layout) { }
		void CreateMarkupMarginNode(XmlNode parent, MarkupMargin markupMargin) { }
		void CreateMarkupLineNode(XmlNode parent, MarkupLine markupLine) { }
		void CreateMarkupCircleNode(XmlNode parent, MarkupCircle markupCircle) { }
		void CreateMarkupRectNode(XmlNode parent, MarkupRect markupRect) { }
		void CreateMarkupEllipseNode(XmlNode parent, MarkupEllipse markupEllipse) { }

		#endregion
	}
}
