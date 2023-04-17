using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace YProgramStudio.ZPLTextEditor
{
	public class Unit
	{
		public string Name { get; set; }

		[XmlIgnore]
		public string Description { get; }

		public Unit(string st)
		{
			this.Name = st;
			string name = this.Name;
			if (!(name == "mm"))
			{
				if (!(name == "in"))
				{
					if (name == "cm")
					{
						this.Description = "Centimeter";
					}
				}
				else
				{
					this.Description = "Inch";
				}
			}
			else
			{
				this.Description = "Millimeter";
			}
			this.Description = "aa";
		}

		public Unit()
		{
		}

		public static List<Unit> GetAvailableUnit()
		{
			return new List<Unit>
			{
				new Unit("in"),
				new Unit("cm"),
				new Unit("mm")
			};
		}
	}
}
