using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Backends.Barcode
{
	/// <summary>
	/// 条码样式
	/// </summary>
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

		public Style()
		{
			_id = string.Empty;
			_backendId = string.Empty;
			_name = string.Empty;
			_canText = false;
			_textOptional = false;
			_canChecksum = false;
			_checksumOptional = false;
			_defaultDigits = string.Empty;
			_canFreeform = false;
			_preferedN = 0;
		}

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
			_id = id;
			_backendId = backendId;
			_name = name;
			_canText = canText;
			_textOptional = textOptional;
			_canChecksum = canChecksum;
			_checksumOptional = checksumOptional;
			_defaultDigits = defaultDigits;
			_canFreeform = canFreeform;
			_preferedN = preferedN;
		}

		public string Id { get => _id; set => _id = value; }

		public string BackendId { get => _backendId; set => _backendId = value; }

		public string Name { get => _name; set => _name = value; }

		/// Full ID Property Getter
		public string FullId { get => string.IsNullOrEmpty(_backendId) ? _id : _backendId + "::" + _id; }

		public string DefaultDigits { get => _defaultDigits; set => _defaultDigits = value; }

		public bool CanText { get => _canText; set => _canText = value; }

		public bool TextOptional { get => _textOptional; set => _textOptional = value; }

		public bool CanChecksum { get => _canChecksum; set => _canChecksum = value; }

		public bool ChecksumOptional { get => _checksumOptional; set => _checksumOptional = value; }

		public bool CanFreeform { get => _canFreeform; set => _canFreeform = value; }

		public int PreferedN { get => _preferedN; set => _preferedN = value; }

	}
}
