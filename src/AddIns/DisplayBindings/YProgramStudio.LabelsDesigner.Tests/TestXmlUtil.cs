using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml;
using YProgramStudio.LabelsDesigner.Model;

namespace YProgramStudio.LabelsDesigner.Tests
{
	[TestClass]
	public class TestXmlUtil
	{
		[TestMethod]
		public void GetStringAttr()
		{
			// Test XML
			string xml = "<root a='A test string' />";

			// Setup document and extract root node
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(xml);
			var node = doc.DocumentElement;

			Assert.AreEqual(node.Name, "root");

			//
			// Tests
			//
			Assert.AreEqual(XmlUtil.GetStringAttr(node, "a", "Default"), "A test string");

			// non-existant attribute, use default
			Assert.AreEqual(XmlUtil.GetStringAttr(node, "b", "Default"), "Default");
		}

		[TestMethod]
		public void GetDoubleAttr()
		{
			// Test XML
			string xml = "<root a='0' b='0.' c='1' d='1.5' e='-1.5e-1' f='x' />";

			// Setup document and extract root node
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(xml);
			var node = doc.DocumentElement;
			Assert.AreEqual(node.Name, "root");

			//
			// Tests
			//
			Assert.AreEqual(XmlUtil.GetFloatAttr(node, "a", 3.14f), 0.0);
			Assert.AreEqual(XmlUtil.GetFloatAttr(node, "b", 3.14f), 0.0);
			Assert.AreEqual(XmlUtil.GetFloatAttr(node, "c", 3.14f), 1.0);
			Assert.AreEqual(XmlUtil.GetFloatAttr(node, "d", 3.14f), 1.5);
			Assert.AreEqual(XmlUtil.GetFloatAttr(node, "e", 3.14f), -0.15);

			// bad value, use default
			Assert.AreEqual(XmlUtil.GetFloatAttr(node, "f", 3.14f), 3.14);

			// non-existant attribute, use default
			Assert.AreEqual(XmlUtil.GetFloatAttr(node, "g", 3.14f), 3.14);
		}

		[TestMethod]
		public void GetBoolAttr()
		{
			// Test XML
			string xml = "<root a='0' b='1' c='true' d='false' e='x' />";

			// Setup document and extract root node
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(xml);
			var node = doc.DocumentElement;
			Assert.AreEqual(node.Name, "root");

			//
			// Tests
			//
			Assert.AreEqual(XmlUtil.GetBoolAttr(node, "a", false), false);
			Assert.AreEqual(XmlUtil.GetBoolAttr(node, "a", true), false);
			Assert.AreEqual(XmlUtil.GetBoolAttr(node, "b", false), true);
			Assert.AreEqual(XmlUtil.GetBoolAttr(node, "b", true), true);
			Assert.AreEqual(XmlUtil.GetBoolAttr(node, "c", false), true);
			Assert.AreEqual(XmlUtil.GetBoolAttr(node, "c", true), true);
			Assert.AreEqual(XmlUtil.GetBoolAttr(node, "d", false), false);
			Assert.AreEqual(XmlUtil.GetBoolAttr(node, "d", true), false);

			// bad value, use default
			Assert.AreEqual(XmlUtil.GetBoolAttr(node, "e", false), false);
			Assert.AreEqual(XmlUtil.GetBoolAttr(node, "e", true), true);

			// non-existant attribute, use default
			Assert.AreEqual(XmlUtil.GetBoolAttr(node, "f", false), false);
			Assert.AreEqual(XmlUtil.GetBoolAttr(node, "f", true), true);
		}

		[TestMethod]
		public void GetLengthAttr()
		{
			// Test XML
			string xml = "<root a='0' b='-1' c='10in' d='1.5mm' e='-1.5e-1cm' f='3pt' g='1.2bad' h='x' />";

			// Setup document and extract root node
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(xml);
			var node = doc.DocumentElement;
			Assert.AreEqual(node.Name, "root");

			//
			// Tests
			//
			Assert.AreEqual(XmlUtil.GetLengthAttr(node, "a", Distance.Pt(1234)), Distance.Pt(0.0f));
			Assert.AreEqual(XmlUtil.GetLengthAttr(node, "b", Distance.Pt(1234)), Distance.Pt(-1.0f));
			Assert.AreEqual(XmlUtil.GetLengthAttr(node, "c", Distance.Pt(1234)), Distance.Pt(720));
			Assert.AreEqual(XmlUtil.GetLengthAttr(node, "d", Distance.Pt(1234)), Distance.Mm(1.5f));
			Assert.AreEqual(XmlUtil.GetLengthAttr(node, "e", Distance.Pt(1234)), Distance.Cm(-0.15f));
			Assert.AreEqual(XmlUtil.GetLengthAttr(node, "f", Distance.Pt(1234)), Distance.Pt(3));

			// bad value, use default
			Assert.AreEqual(XmlUtil.GetLengthAttr(node, "g", Distance.Pt(1234)), Distance.Pt(1234));
			Assert.AreEqual(XmlUtil.GetLengthAttr(node, "h", Distance.Pt(1234)), Distance.Pt(1234));

			// non-existant attribute, use default
			Assert.AreEqual(XmlUtil.GetLengthAttr(node, "i", Distance.Pt(1234)), Distance.Pt(1234));
		}

	}
}
