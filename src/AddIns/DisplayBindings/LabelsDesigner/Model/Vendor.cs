using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 供应商
	/// </summary>
	public class Vendor
	{
		public Vendor(string name, string url)
		{
			Name = name;
			Url = url;
		}

		public string Name { get; private set; }
		public string Url { get; private set; }
	}
}
