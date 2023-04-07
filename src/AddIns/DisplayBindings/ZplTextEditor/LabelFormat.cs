using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace VisualLabelDesigner.ZplTextEditor
{
	public class LabelFormat
	{
		private const string FILE_PATH = "\\include\\LabelFormat.xml";
		private Measure _height;

		public string Name { get; set; }

		public string DisplayValue { get; set; }

		public override string ToString()
		{
			return DisplayValue;
		}

		public Measure Width { get; set; }

		public Measure Height
		{
			get
			{
				return this._height;
			}
			set
			{
				this._height = value;
				double value2 = this.Height.Value;
				if (0.0.Equals(value2))
				{
					this.DisplayValue = this.Name;
					return;
				}
				this.DisplayValue = string.Format("{0} - [{1} x {2} {3}]", new object[]
				{
					this.Name,
					this.Width.Value.ToString(),
					this.Height.Value.ToString(),
					this.Height.Unit.Name
				});
			}
		}

		public static List<LabelFormat> DeserializeFromXML()
		{
			string text = string.Empty;
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<LabelFormat>));
			List<LabelFormat> result = new List<LabelFormat>();
			if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + FILE_PATH))
			{
				text = AppDomain.CurrentDomain.BaseDirectory + FILE_PATH;
			}
			if (string.IsNullOrEmpty(text) && File.Exists(Application.LocalUserAppDataPath + FILE_PATH))
			{
				text = Application.LocalUserAppDataPath + FILE_PATH;
			}
			if (!string.IsNullOrEmpty(text))
			{
				using (TextReader textReader = new StreamReader(text))
				{
					result = (List<LabelFormat>)xmlSerializer.Deserialize(textReader);
				}
			}
			return result;
		}

		public static bool SerializeToXML(List<LabelFormat> li)
		{
			string text = string.Empty;
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<LabelFormat>));
			if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + FILE_PATH))
			{
				text = AppDomain.CurrentDomain.BaseDirectory + FILE_PATH;
			}
			if (string.IsNullOrEmpty(text) && File.Exists(Application.LocalUserAppDataPath + FILE_PATH))
			{
				text = Application.LocalUserAppDataPath + FILE_PATH;
			}
			using (TextWriter textWriter = new StreamWriter(text))
			{
				xmlSerializer.Serialize(textWriter, li);
			}
			return true;
		}
	}
}
