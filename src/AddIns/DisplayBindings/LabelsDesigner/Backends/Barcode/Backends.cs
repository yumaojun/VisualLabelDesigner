using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Backends.Barcode
{
	public class Backends
	{
		private static Backends singletonInstance = null;

		private static List<Style> _styleList = new List<Style>();

		private Backends() { }

		public static void Init()
		{
			if (singletonInstance == null)
			{
				singletonInstance = new Backends();
			}
		}

		public static Style DefaultStyle()
		{
			return _styleList.First();
		}

		public static Style Style(string backendId, string styleId)
		{
			foreach (Style bcStyle in _styleList)
			{
				if ((bcStyle.BackendId == backendId) && (bcStyle.Id == styleId))
				{
					return bcStyle;
				}
			}

			return DefaultStyle();
		}
	}
}
