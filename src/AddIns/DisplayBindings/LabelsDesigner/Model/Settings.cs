using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	public enum PageSizeFamily { ISO, US, };

	public class Settings
	{
		private static Settings _instance;

		private Settings()
		{
		}

		static void Init()
		{
			if (_instance == null)
			{
				_instance = new Settings();
			}
		}

		public static Settings Instance()
		{
			Init();

			return _instance;
		}

		public string Value(string units, string id)
		{
			return "";
		}

		public static Units Units()
		{
			// Guess at a suitable default
			string defaultIdString;
			//if (QLocale::system().measurementSystem() == QLocale::ImperialSystem)
			{
				defaultIdString = new Units(UnitEnum.IN).ToIdString();
			}
			//else
			//{
			//	defaultIdString = Units(Units::MM).toIdString();
			//}

			//_instance->beginGroup("Locale");
			string idString = _instance.Value("units", defaultIdString).ToString();
			//_instance->endGroup();

			return new Units(idString);
		}
	}
}
