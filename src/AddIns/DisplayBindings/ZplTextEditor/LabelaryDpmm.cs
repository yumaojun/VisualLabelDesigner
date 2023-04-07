using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualLabelDesigner.ZplTextEditor
{
	public class LabelaryDpmm
	{
		public string Code { get; set; }

		public string Description { get; set; }

		public override string ToString()
		{
			return Description;
		}

		public LabelaryDpmm(string a, string b)
		{
			this.Code = a;
			this.Description = b;
		}

		public static List<LabelaryDpmm> Data()
		{
			List<LabelaryDpmm> list = new List<LabelaryDpmm>();
			LabelaryDpmm item1 = new LabelaryDpmm("6dpmm", "6 dpmm (150 dpi)");
			LabelaryDpmm item2 = new LabelaryDpmm("8dpmm", "8 dpmm (203 dpi)");
			LabelaryDpmm item3 = new LabelaryDpmm("12dpmm", "12 dpmm (300 dpi)");
			LabelaryDpmm item4 = new LabelaryDpmm("24dpmm", "24 dpmm (600 dpi)");
			list.Add(item1);
			list.Add(item2);
			list.Add(item3);
			list.Add(item4);
			return list;
		}
	}
}
