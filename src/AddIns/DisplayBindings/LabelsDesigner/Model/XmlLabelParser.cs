using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ICSharpCode.SharpDevelop.Workbench;
using SkiaSharp;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// Label Parser from Xml file
	/// </summary>
	public static class XmlLabelParser
	{
		public static Model ReadFile(string fileName, Stream stream)
		{
			XmlDocument doc = new XmlDocument();

			using (StreamReader reader = new StreamReader(stream))
			{
				doc.LoadXml(reader.ReadToEnd());
			}

			var root = doc.DocumentElement;
			if (root.Name != "Glabels-document")
			{
				return null;
			}

			return ParseRootNode(root, fileName);
		}

		public static Model ReadFile(OpenedFile file, Stream stream)
		{
			XmlDocument doc = new XmlDocument();

			using (StreamReader reader = new StreamReader(stream))
			{
				doc.LoadXml(reader.ReadToEnd());
			}

			var root = doc.DocumentElement;
			if (root.Name != "Glabels-document")
			{
				return null;
			}

			return ParseRootNode(root, file.FileName);
		}

		private static Model ParseRootNode(XmlNode node, string fileName)
		{
			string version = XmlUtil.GetStringAttr(node, "version", string.Empty);
			if (version != "4.0")
			{
				// Attempt to import as version 3.0 format (glabels 2.0 - glabels 3.4)
				var model1 = XmlLabelParser_3.ParseRootNode(node);
				if (model1 != null)
				{
					model1.FileName = fileName;
				}
				return model1;
			}

			var model = new Model() { FileName = fileName };

			/* Pass 1, extract data nodes to pre-load cache. */
			DataCache data = new DataCache();
			for (XmlNode child = node.FirstChild; child != null; child = child.NextSibling)
			{
				if (child.Name == "Data")
				{
					ParseDataNode(child, model, data);
				}
			}

			/* Pass 2, now extract everything else. */
			for (XmlNode child = node.FirstChild; child != null; child = child.NextSibling)
			{
				if (child.Name == "Template")
				{
					Template template = new XmlTemplateParser().ParseTemplateNode(child); // TODO: *new XmlTemplateParser()可能需要优化?
					if (template == null)
					{
						return null;
					}
					model.Template = template; // Copies arg
				}
				else if (child.Name == "Objects")
				{
					model.Rotate = ParseRotateAttr(child);
					var list = ParseObjectsNode(child, model, data);
					foreach (ModelObject modelObject in list)
					{
						model.AddObject(modelObject);
					}
				}
				else if (child.Name == "Merge")
				{
					ParseMergeNode(child, model);
				}
				else if (child.Name == "Variables")
				{
					ParseVariablesNode(child, model);
				}
				else if (child.Name == "Data")
				{
					/* Handled in pass 1. */
				}
				//else if (!child.isComment())
				//{
				//	"Unexpected" << node.tagName() << "child:" << tagName;
				//}
			}

			model.Modified = false; // TODO: * model.Modified = false 需要优化
			return model;
		}

		private static List<ModelObject> ParseObjectsNode(XmlNode node, Model model, DataCache data)
		{
			List<ModelObject> list = new List<ModelObject>();

			for (XmlNode child = node.FirstChild; child != null; child = child.NextSibling)
			{
				if (child.Name == "Object-box")
				{
					list.Add(ParseObjectBoxNode(child));
				}
				else if (child.Name == "Object-ellipse")
				{
					list.Add(ParseObjectEllipseNode(child));
				}
				else if (child.Name == "Object-line")
				{
					list.Add(ParseObjectLineNode(child));
				}
				else if (child.Name == "Object-text")
				{
					list.Add(ParseObjectTextNode(child));
				}
				else if (child.Name == "Object-image")
				{
					list.Add(ParseObjectImageNode(child, model, data));
				}
				else if (child.Name == "Object-barcode")
				{
					list.Add(ParseObjectBarcodeNode(child));
				}
			}

			return list;
		}

		/// <summary>
		/// 方框
		/// </summary>
		private static ModelBoxObject ParseObjectBoxNode(XmlNode node)
		{
			/* position attrs */
			Distance x0 = XmlUtil.GetLengthAttr(node, "x", 0.0f);
			Distance y0 = XmlUtil.GetLengthAttr(node, "y", 0.0f);

			/* size attrs */
			Distance w = XmlUtil.GetLengthAttr(node, "w", 0);
			Distance h = XmlUtil.GetLengthAttr(node, "h", 0);
			bool lockAspectRatio = XmlUtil.GetBoolAttr(node, "lock_aspect_ratio", false);

			/* line attrs */
			Distance lineWidth = XmlUtil.GetLengthAttr(node, "line_width", 1.0f);

			string key = XmlUtil.GetStringAttr(node, "line_color_field", string.Empty);
			bool field_flag = !string.IsNullOrEmpty(key);
			uint color = XmlUtil.GetUIntAttr(node, "line_color", 0xFF);
			ColorNode lineColorNode = new ColorNode(field_flag, color, key);

			/* fill attrs */
			key = XmlUtil.GetStringAttr(node, "fill_color_field", string.Empty);
			field_flag = !string.IsNullOrEmpty(key);
			color = XmlUtil.GetUIntAttr(node, "fill_color", 0xFF);
			ColorNode fillColorNode = new ColorNode(field_flag, color, key);

			/* affine attrs */
			float[] a = new float[9];
			a[0] = XmlUtil.GetFloatAttr(node, "a0", 1.0f); // m11
			a[1] = XmlUtil.GetFloatAttr(node, "a1", 0.0f); // m12
			a[2] = 0.0f;
			a[3] = XmlUtil.GetFloatAttr(node, "a2", 0.0f); // m21
			a[4] = XmlUtil.GetFloatAttr(node, "a3", 1.0f); // m22
			a[5] = 0.0f;
			a[6] = XmlUtil.GetFloatAttr(node, "a4", 0.0f); // dx
			a[7] = XmlUtil.GetFloatAttr(node, "a5", 0.0f); // dy
			a[8] = 1.0f;

			/* shadow attrs */
			bool shadowState = XmlUtil.GetBoolAttr(node, "shadow", false);
			Distance shadowX = XmlUtil.GetLengthAttr(node, "shadow_x", 0.0f);
			Distance shadowY = XmlUtil.GetLengthAttr(node, "shadow_y", 0.0f);
			float shadowOpacity = XmlUtil.GetFloatAttr(node, "shadow_opacity", 1.0f);

			key = XmlUtil.GetStringAttr(node, "shadow_color_field", string.Empty);
			field_flag = !string.IsNullOrEmpty(key);
			color = XmlUtil.GetUIntAttr(node, "shadow_color", 0xFF);
			ColorNode shadowColorNode = new ColorNode(field_flag, color, key);

			return new ModelBoxObject(x0, y0, w, h, lockAspectRatio,
									   lineWidth, lineColorNode,
									   fillColorNode,
									   new SKMatrix(a),
									   shadowState, shadowX, shadowY, shadowOpacity, shadowColorNode);
		}

		/// <summary>
		/// 椭圆、圆
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		private static ModelEllipseObject ParseObjectEllipseNode(XmlNode node)
		{
			/* position attrs */
			Distance x0 = XmlUtil.GetLengthAttr(node, "x", 0.0f);
			Distance y0 = XmlUtil.GetLengthAttr(node, "y", 0.0f);

			/* size attrs */
			Distance w = XmlUtil.GetLengthAttr(node, "w", 0);
			Distance h = XmlUtil.GetLengthAttr(node, "h", 0);
			bool lockAspectRatio = XmlUtil.GetBoolAttr(node, "lock_aspect_ratio", false);

			/* line attrs */
			Distance lineWidth = XmlUtil.GetLengthAttr(node, "line_width", 1.0f);

			string key = XmlUtil.GetStringAttr(node, "line_color_field", string.Empty);
			bool field_flag = !string.IsNullOrEmpty(key);
			uint color = XmlUtil.GetUIntAttr(node, "line_color", 0xFF);
			ColorNode lineColorNode = new ColorNode(field_flag, color, key);

			/* fill attrs */
			key = XmlUtil.GetStringAttr(node, "fill_color_field", string.Empty);
			field_flag = !string.IsNullOrEmpty(key);
			color = XmlUtil.GetUIntAttr(node, "fill_color", 0xFF);
			ColorNode fillColorNode = new ColorNode(field_flag, color, key);

			/* affine attrs */
			float[] a = new float[9];
			a[0] = XmlUtil.GetFloatAttr(node, "a0", 1.0f); // m11
			a[1] = XmlUtil.GetFloatAttr(node, "a1", 0.0f); // m12
			a[2] = 0.0f;
			a[3] = XmlUtil.GetFloatAttr(node, "a2", 0.0f); // m21
			a[4] = XmlUtil.GetFloatAttr(node, "a3", 1.0f); // m22
			a[5] = 0.0f;
			a[6] = XmlUtil.GetFloatAttr(node, "a4", 0.0f); // dx
			a[7] = XmlUtil.GetFloatAttr(node, "a5", 0.0f); // dy
			a[8] = 1.0f;

			/* shadow attrs */
			bool shadowState = XmlUtil.GetBoolAttr(node, "shadow", false);
			Distance shadowX = XmlUtil.GetLengthAttr(node, "shadow_x", 0.0f);
			Distance shadowY = XmlUtil.GetLengthAttr(node, "shadow_y", 0.0f);
			float shadowOpacity = XmlUtil.GetFloatAttr(node, "shadow_opacity", 1.0f);

			key = XmlUtil.GetStringAttr(node, "shadow_color_field", string.Empty);
			field_flag = !string.IsNullOrEmpty(key);
			color = XmlUtil.GetUIntAttr(node, "shadow_color", 0xFF);
			ColorNode shadowColorNode = new ColorNode(field_flag, color, key);

			return new ModelEllipseObject(x0, y0, w, h, lockAspectRatio,
										   lineWidth, lineColorNode,
										   fillColorNode,
										   new SKMatrix(a),
										   shadowState, shadowX, shadowY, shadowOpacity, shadowColorNode);
		}

		/// <summary>
		/// 线
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		private static ModelLineObject ParseObjectLineNode(XmlNode node)
		{
			/* position attrs */
			Distance x0 = XmlUtil.GetLengthAttr(node, "x", 0.0f);
			Distance y0 = XmlUtil.GetLengthAttr(node, "y", 0.0f);

			/* size attrs of line */
			Distance dx = XmlUtil.GetLengthAttr(node, "dx", 0);
			Distance dy = XmlUtil.GetLengthAttr(node, "dy", 0);

			/* line attrs */
			Distance lineWidth = XmlUtil.GetLengthAttr(node, "line_width", 1.0f);

			string key = XmlUtil.GetStringAttr(node, "line_color_field", string.Empty);
			bool field_flag = !string.IsNullOrEmpty(key);
			uint color = XmlUtil.GetUIntAttr(node, "line_color", 0xFF);
			ColorNode lineColorNode = new ColorNode(field_flag, color, key);

			/* affine attrs */
			float[] a = new float[9];
			a[0] = XmlUtil.GetFloatAttr(node, "a0", 1.0f); // m11
			a[1] = XmlUtil.GetFloatAttr(node, "a1", 0.0f); // m12
			a[2] = 0.0f;
			a[3] = XmlUtil.GetFloatAttr(node, "a2", 0.0f); // m21
			a[4] = XmlUtil.GetFloatAttr(node, "a3", 1.0f); // m22
			a[5] = 0.0f;
			a[6] = XmlUtil.GetFloatAttr(node, "a4", 0.0f); // dx
			a[7] = XmlUtil.GetFloatAttr(node, "a5", 0.0f); // dy
			a[8] = 1.0f;

			/* shadow attrs */
			bool shadowState = XmlUtil.GetBoolAttr(node, "shadow", false);
			Distance shadowX = XmlUtil.GetLengthAttr(node, "shadow_x", 0.0f);
			Distance shadowY = XmlUtil.GetLengthAttr(node, "shadow_y", 0.0f);
			float shadowOpacity = XmlUtil.GetFloatAttr(node, "shadow_opacity", 1.0f);

			key = XmlUtil.GetStringAttr(node, "shadow_color_field", string.Empty);
			field_flag = !string.IsNullOrEmpty(key);
			color = XmlUtil.GetUIntAttr(node, "shadow_color", 0xFF);
			ColorNode shadowColorNode = new ColorNode(field_flag, color, key);

			return new ModelLineObject(x0, y0, dx, dy,
										lineWidth, lineColorNode,
										new SKMatrix(a),
										shadowState, shadowX, shadowY, shadowOpacity, shadowColorNode);
		}

		/// <summary>
		/// 图片
		/// </summary>
		private static ModelImageObject ParseObjectImageNode(XmlNode node, Model model, DataCache data)
		{
			/* position attrs */
			Distance x0 = XmlUtil.GetLengthAttr(node, "x", 0.0f);
			Distance y0 = XmlUtil.GetLengthAttr(node, "y", 0.0f);

			/* size attrs */
			Distance w = XmlUtil.GetLengthAttr(node, "w", 0);
			Distance h = XmlUtil.GetLengthAttr(node, "h", 0);
			bool lockAspectRatio = XmlUtil.GetBoolAttr(node, "lock_aspect_ratio", false);

			/* file attrs */
			string key = XmlUtil.GetStringAttr(node, "src_field", string.Empty);
			bool field_flag = !string.IsNullOrEmpty(key);
			string filename = XmlUtil.GetStringAttr(node, "src", string.Empty);
			TextNode filenameNode = new TextNode(field_flag, field_flag ? key : filename);

			/* affine attrs */
			float[] a = new float[9];
			a[0] = XmlUtil.GetFloatAttr(node, "a0", 1.0f); // m11
			a[1] = XmlUtil.GetFloatAttr(node, "a1", 0.0f); // m12
			a[2] = 0.0f;
			a[3] = XmlUtil.GetFloatAttr(node, "a2", 0.0f); // m21
			a[4] = XmlUtil.GetFloatAttr(node, "a3", 1.0f); // m22
			a[5] = 0.0f;
			a[6] = XmlUtil.GetFloatAttr(node, "a4", 0.0f); // dx
			a[7] = XmlUtil.GetFloatAttr(node, "a5", 0.0f); // dy
			a[8] = 1.0f;

			/* shadow attrs */
			bool shadowState = XmlUtil.GetBoolAttr(node, "shadow", false);
			Distance shadowX = XmlUtil.GetLengthAttr(node, "shadow_x", 0.0f);
			Distance shadowY = XmlUtil.GetLengthAttr(node, "shadow_y", 0.0f);
			float shadowOpacity = XmlUtil.GetFloatAttr(node, "shadow_opacity", 1.0f);

			key = XmlUtil.GetStringAttr(node, "shadow_color_field", string.Empty);
			field_flag = !string.IsNullOrEmpty(key);
			uint color = XmlUtil.GetUIntAttr(node, "shadow_color", 0xFF);
			ColorNode shadowColorNode = new ColorNode(field_flag, color, key);

			if (filenameNode.IsField)
			{
				return new ModelImageObject(x0, y0, w, h, lockAspectRatio,
											 filenameNode,
											 new SKMatrix(a),
											 shadowState, shadowX, shadowY, shadowOpacity, shadowColorNode);
			}
			else
			{
				string fn = model.Dir(); // TODO: QDir::cleanPath(model->dir().absoluteFilePath(filename));

				if (data.HasImage(fn))
				{
					return new ModelImageObject(x0, y0, w, h, lockAspectRatio,
												 filename, data.GetImage(fn),
												 new SKMatrix(a),
												 shadowState, shadowX, shadowY, shadowOpacity, shadowColorNode);
				}
				else if (data.HasSvg(fn))
				{
					return new ModelImageObject(x0, y0, w, h, lockAspectRatio,
												 filename, data.GetSvg(fn),
												 new SKMatrix(a),
												 shadowState, shadowX, shadowY, shadowOpacity, shadowColorNode);
				}
				else
				{
					if (!string.IsNullOrEmpty(filename))
					{
						filenameNode.Data = fn; // "Embedded file" << fn << "missing. Trying actual file.";
					}
					return new ModelImageObject(x0, y0, w, h, lockAspectRatio,
												 filenameNode,
												 new SKMatrix(a),
												 shadowState, shadowX, shadowY, shadowOpacity, shadowColorNode);
				}
			}
		}

		/// <summary>
		/// 条码
		/// </summary>
		private static ModelBarcodeObject ParseObjectBarcodeNode(XmlNode node)
		{
			/* position attrs */
			Distance x0 = XmlUtil.GetLengthAttr(node, "x", 0.0f);
			Distance y0 = XmlUtil.GetLengthAttr(node, "y", 0.0f);

			/* size attrs */
			Distance w = XmlUtil.GetLengthAttr(node, "w", 0);
			Distance h = XmlUtil.GetLengthAttr(node, "h", 0);
			bool lockAspectRatio = XmlUtil.GetBoolAttr(node, "lock_aspect_ratio", false);

			/* barcode attrs */
			Backends.Barcode.BarcodeStyle bcStyle = Backends.Barcode.Backends.Style(XmlUtil.GetStringAttr(node, "backend", string.Empty),
															   XmlUtil.GetStringAttr(node, "style", string.Empty));
			bool bcTextFlag = XmlUtil.GetBoolAttr(node, "text", true);
			bool bcChecksumFlag = XmlUtil.GetBoolAttr(node, "checksum", true);

			string key = XmlUtil.GetStringAttr(node, "color_field", string.Empty);
			bool field_flag = !string.IsNullOrEmpty(key);
			uint color = XmlUtil.GetUIntAttr(node, "color", 0xFF);
			ColorNode bcColorNode = new ColorNode(field_flag, color, key);

			string bcData = XmlUtil.GetStringAttr(node, "data", string.Empty);

			/* affine attrs */
			float[] a = new float[9];
			a[0] = XmlUtil.GetFloatAttr(node, "a0", 1.0f); // m11
			a[1] = XmlUtil.GetFloatAttr(node, "a1", 0.0f); // m12
			a[2] = 0.0f;
			a[3] = XmlUtil.GetFloatAttr(node, "a2", 0.0f); // m21
			a[4] = XmlUtil.GetFloatAttr(node, "a3", 1.0f); // m22
			a[5] = 0.0f;
			a[6] = XmlUtil.GetFloatAttr(node, "a4", 0.0f); // dx
			a[7] = XmlUtil.GetFloatAttr(node, "a5", 0.0f); // dy
			a[8] = 1.0f;

			return new ModelBarcodeObject(x0, y0, w, h, lockAspectRatio,
										  bcStyle, bcTextFlag, bcChecksumFlag, bcData, bcColorNode,
										  new SKMatrix(a));
		}

		/// <summary>
		/// 文本
		/// </summary>
		private static ModelTextObject ParseObjectTextNode(XmlNode node)
		{
			/* position attrs */
			Distance x0 = XmlUtil.GetLengthAttr(node, "x", 0.0f);
			Distance y0 = XmlUtil.GetLengthAttr(node, "y", 0.0f);

			/* size attrs */
			Distance w = XmlUtil.GetLengthAttr(node, "w", 0);
			Distance h = XmlUtil.GetLengthAttr(node, "h", 0);
			bool lockAspectRatio = XmlUtil.GetBoolAttr(node, "lock_aspect_ratio", false);

			/* color attr */
			string key = XmlUtil.GetStringAttr(node, "color_field", string.Empty);
			bool field_flag = !string.IsNullOrEmpty(key);
			uint color = XmlUtil.GetUIntAttr(node, "color", 0xFF);
			ColorNode textColorNode = new ColorNode(field_flag, color, key);

			/* font attrs */
			string fontFamily = XmlUtil.GetStringAttr(node, "font_family", "Sans");
			float fontSize = XmlUtil.GetFloatAttr(node, "font_size", 10);
			SKFontStyleWeight fontWeight = XmlUtil.GetWeightAttr(node, "font_weight", SKFontStyleWeight.Normal);
			bool fontItalicFlag = XmlUtil.GetBoolAttr(node, "font_italic", false);
			bool fontUnderlineFlag = XmlUtil.GetBoolAttr(node, "font_underline", false);

			/* text attrs */
			float textLineSpacing = XmlUtil.GetFloatAttr(node, "line_spacing", 1);
			Alignment textHAlign = XmlUtil.GetAlignmentAttr(node, "align", Alignment.Left);
			Alignment textVAlign = XmlUtil.GetAlignmentAttr(node, "valign", Alignment.Top);
			WrapMode textWrapMode = XmlUtil.GetWrapModeAttr(node, "wrap", WrapMode.WordWrap);
			bool textAutoShrink = XmlUtil.GetBoolAttr(node, "auto_shrink", false);

			/* affine attrs */
			float[] a = new float[9];
			a[0] = XmlUtil.GetFloatAttr(node, "a0", 1.0f); // m11
			a[1] = XmlUtil.GetFloatAttr(node, "a1", 0.0f); // m12
			a[2] = 0.0f;
			a[3] = XmlUtil.GetFloatAttr(node, "a2", 0.0f); // m21
			a[4] = XmlUtil.GetFloatAttr(node, "a3", 1.0f); // m22
			a[5] = 0.0f;
			a[6] = XmlUtil.GetFloatAttr(node, "a4", 0.0f); // dx
			a[7] = XmlUtil.GetFloatAttr(node, "a5", 0.0f); // dy
			a[8] = 1.0f;

			/* shadow attrs */
			bool shadowState = XmlUtil.GetBoolAttr(node, "shadow", false);
			Distance shadowX = XmlUtil.GetLengthAttr(node, "shadow_x", 0.0f);
			Distance shadowY = XmlUtil.GetLengthAttr(node, "shadow_y", 0.0f);
			float shadowOpacity = XmlUtil.GetFloatAttr(node, "shadow_opacity", 1.0f);

			key = XmlUtil.GetStringAttr(node, "shadow_color_field", string.Empty);
			field_flag = !string.IsNullOrEmpty(key);
			color = XmlUtil.GetUIntAttr(node, "shadow_color", 0xFF);
			ColorNode shadowColorNode = new ColorNode(field_flag, color, key);

			/* deserialize contents. */
			TextDocument document = new TextDocument();
			TextCursor cursor = new TextCursor(ref document);
			bool firstBlock = true;
			for (XmlNode child = node.FirstChild; child != null; child = child.NextSibling)
			{
				if (child.Name == "p")
				{
					if (!firstBlock)
					{
						cursor.InsertBlock(); // 文本1\n文本2\n文本3
					}
					firstBlock = false;
					cursor.InsertText(ParsePNode(child));
				}
			}
			string text = document.PlainText();

			return new ModelTextObject(x0, y0, w, h, lockAspectRatio,
										text,
										fontFamily, fontSize, fontWeight, fontItalicFlag, fontUnderlineFlag,
										textColorNode, textHAlign, textVAlign, textWrapMode, textLineSpacing,
										textAutoShrink,
										new SKMatrix(a),
										shadowState, shadowX, shadowY, shadowOpacity, shadowColorNode);
		}

		private static string ParsePNode(XmlNode node)
		{
			return node.InnerText;
		}

		private static bool ParseRotateAttr(XmlNode node)
		{
			return XmlUtil.GetBoolAttr(node, "rotate", false);
		}

		private static void ParseMergeNode(XmlNode node, Model model)
		{
			string id = XmlUtil.GetStringAttr(node, "type", "None");
			string src = XmlUtil.GetStringAttr(node, "src", string.Empty);

			Backends.Merge.Merge merge = Backends.Merge.Factory.CreateMerge(id);

			//		switch (merge::Factory::idToType(id ) )
			//		{
			//		case merge::Factory::NONE:
			//		case merge::Factory::FIXED:
			//			break;

			//		case merge::Factory::FILE:
			//			{
			//				QString fn = QDir::cleanPath(model->dir().absoluteFilePath(src));
			//	merge->setSource(fn );
			//}
			//			break;

			//		default:
			//			qWarning() << "XmlLabelParser::parseMergeNode(): Should not be reached!";
			//			break;
			//		}

			//           model.SetMerge(merge);
		}

		private static void ParseVariablesNode(XmlNode node, Model model)
		{
			for (XmlNode child = node.FirstChild; child != null; child = child.NextSibling)
			{
				if (child.Name == "Variable")
				{
					ParseVariableNode(child, model);
				}
			}
		}

		private static void ParseVariableNode(XmlNode node, Model model)
		{
			string typeString = XmlUtil.GetStringAttr(node, "type", "string");
			string name = XmlUtil.GetStringAttr(node, "name", "unknown");
			string initialValue = XmlUtil.GetStringAttr(node, "initialValue", "0");
			string incrementString = XmlUtil.GetStringAttr(node, "increment", "never");
			string stepSize = XmlUtil.GetStringAttr(node, "stepSize", "0");

			var type = Variable.IdStringToType(typeString);
			var increment = Variable.IdStringToIncrement(incrementString);

			Variable v = new Variable(type, name, initialValue, increment, stepSize);
			//model.variables()->addVariable(v );
		}

		private static void ParseDataNode(XmlNode node, Model model, DataCache data)
		{
			for (XmlNode child = node.FirstChild; child != null; child = child.NextSibling)
			{
				if (child.Name == "File")
				{
					ParseFileNode(child, model, data);
				}
			}
		}

		private static void ParseFileNode(XmlNode node, Model model, DataCache data)
		{
			string name = XmlUtil.GetStringAttr(node, "name", string.Empty);
			string mimetype = XmlUtil.GetStringAttr(node, "mimetype", "image/png");
			string encoding = XmlUtil.GetStringAttr(node, "encoding", "base64");

			// Rewrite name as absolute file path
			string fn = model.Dir(); //QDir::cleanPath(model->dir().absoluteFilePath(name));

			if (mimetype == "image/png")
			{
				if (encoding == "base64")
				{
					byte[] ba64 = Encoding.UTF8.GetBytes(node.InnerText);
					//byte[] ba = QByteArray::fromBase64(ba64);
					SKImage image = SKImage.FromEncodedData(ba64); // TODO: *ParseFileNode()=>QByteArray::fromBase64(ba64)这里是ba
																   //image..loadFromData(ba, "PNG");

					data.AddImage(fn, image);
				}
			}
			else if (mimetype == "image/svg+xml")
			{
				data.AddSvg(fn, Encoding.UTF8.GetBytes(node.InnerText));
			}
		}
	}
}
