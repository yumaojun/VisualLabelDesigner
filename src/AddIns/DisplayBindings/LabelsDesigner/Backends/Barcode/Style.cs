using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Backends.Barcode
{
	public class Style
	{
		private string _id;
		private string _backendId;
		private string _name;
		private bool _canText;
		private bool _textOptional;
		private bool _canChecksum;
		private bool _checksumOptional;
		private string _defaultDigits;
		private bool _canFreeform;
		private int _preferedN;

		public Style() { }

		public Style(string id,
					 string backendId,
					 string name,
					 bool canText,

					 bool textOptional,
					 bool canChecksum,
					 bool checksumOptional,

					 string defaultDigits,
					 bool canFreeform,

					 int preferedN)
		{
		}

		public string Id { get => _id; set => _id = value; }

		public string BackendId { get => _backendId; set => _backendId = value; }
	}
}
