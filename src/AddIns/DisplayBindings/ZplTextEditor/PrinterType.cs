using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.ZPLTextEditor
{
	public class PrinterType
	{
		public string Code { get; set; }

		public string Description { get; set; }

		public override string ToString()
		{
			return Description;
		}

		public PrinterType(string a, string b)
		{
			this.Code = a;
			this.Description = b;
		}

		public static List<PrinterType> Data()
		{
			List<PrinterType> list = new List<PrinterType>();
			PrinterType item1 = new PrinterType("0", "Local Zebra Printer");
			PrinterType item2 = new PrinterType("1", "Labelary Web");
			list.Add(item1);
			list.Add(item2);
			return list;
		}
	}
}
