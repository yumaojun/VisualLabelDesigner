using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VisualLabelDesigner.ZplTextEditor.Services
{
	[Serializable]
	public class ZPLCommand
	{
		[XmlElement("Name")]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		[XmlElement("Category")]
		public string Category
		{
			get
			{
				return this._category;
			}
			set
			{
				this._category = value;
			}
		}

		[XmlElement("ShortDesc")]
		public string ShortDescription
		{
			get
			{
				return this._short_desc;
			}
			set
			{
				this._short_desc = value;
			}
		}

		[XmlElement("LongDesc")]
		public string LongDescription
		{
			get
			{
				return this._long_desc;
			}
			set
			{
				this._long_desc = value;
			}
		}

		[XmlElement("Usage")]
		public string Usage
		{
			get
			{
				return this._usage;
			}
			set
			{
				this._usage = value;
			}
		}

		[XmlElement("Assistant")]
		public string Assistant
		{
			get
			{
				return this._assistant;
			}
			set
			{
				this._assistant = value;
			}
		}

		[XmlElement("Command")]
		public string Command
		{
			get
			{
				return this._command;
			}
			set
			{
				this._command = value;
			}
		}

		[XmlElement("ZPLCommandParameter")]
		public ZPLCommandParameter Parameters
		{
			get
			{
				return this._l_parameters;
			}
			set
			{
				this._l_parameters = value;
			}
		}

		public static List<ZPLCommand> DeserializeFromXML()
		{
			string text = string.Empty;
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ZPLCommand>));
			List<ZPLCommand> list = new List<ZPLCommand>();
			if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\include\\ZPHelp.xml"))
			{
				text = AppDomain.CurrentDomain.BaseDirectory + "\\include\\ZPHelp.xml";
			}
			if (string.IsNullOrEmpty(text) && File.Exists(System.Windows.Forms.Application.LocalUserAppDataPath + "\\include\\ZPHelp.xml"))
			{
				text = System.Windows.Forms.Application.LocalUserAppDataPath + "\\include\\ZPHelp.xml";
			}
			List<ZPLCommand> result;
			try
			{
				if (!string.IsNullOrEmpty(text))
				{
					using (TextReader textReader = new StreamReader(text))
					{
						list = (List<ZPLCommand>)xmlSerializer.Deserialize(textReader);
					}
				}
				result = list;
			}
			catch (Exception)
			{
				throw new Exception();
			}
			return result;
		}

		public static void SerializeToXML(List<ZPLCommand> co)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ZPLCommand>));
			using (StreamWriter streamWriter = new StreamWriter("c:\\person.xml"))
			{
				xmlSerializer.Serialize(streamWriter, co);
			}
		}

		private string _name;

		private string _category;

		private string _short_desc;

		private string _long_desc;

		private string _usage;

		private string _assistant;

		private string _command;

		private ZPLCommandParameter _l_parameters;
	}
}
