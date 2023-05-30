using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Backends.Merge
{
	public class Record : Dictionary<string, string>
	{
		public bool Selected { get; set; }

		///
		/// Constructor
		///
		public Record()
		{
			Selected = true;
		}

		///
		/// Constructor
		///
		public Record(Record record)
		{
			foreach (var key in record.Keys)
			{
				Add(key, record[key]);
			}
			Selected = record.Selected;
		}

		/// Clone
		public Record Clone()
		{
			return new Record(this);
		}
	}
}
