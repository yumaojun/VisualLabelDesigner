using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YProgramStudio.LabelsDesigner.Backends.Merge;

namespace YProgramStudio.LabelsDesigner.Model
{
	///
	/// Text Node Type
	///
	public struct TextNode
	{
		private bool _isField;
		private string _data;

		// Life Cycle
		public TextNode(bool isField = false, string data = "")
		{
			_isField = isField;
			_data = data;
		}

		// Properties
		//
		// is field? Property
		//
		public bool IsField
		{
			get => _isField;
			set => _isField = value;
		}

		//
		// Data Property
		//
		public string Data
		{
			get => _data;
			set => _data = value;
		}

		// Misc. Methods
		public string Text(Record record, Variables variables)
		{
			string value = string.Empty;
			bool haveRecordField = IsField && record != null && record.ContainsKey(Data) && !string.IsNullOrEmpty(record[Data]);
			bool haveVariable = IsField && variables != null && variables.ContainsKey(Data) && !string.IsNullOrEmpty(variables[Data].Value());

			if (haveRecordField)
			{
				value = record[Data];
			}
			else if (haveVariable)
			{
				value = variables[Data].Value();
			}
			else if (!IsField)
			{
				value = Data;
			}

			return value;
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return IsField.GetHashCode() + Data.GetHashCode(); // TODO: *重载GetHashCode
		}

		// Operators
		public static bool operator ==(TextNode a, TextNode other) => (a.IsField == other.IsField) && (a.Data == other.Data);

		public static bool operator !=(TextNode a, TextNode other) => (a.IsField != other.IsField) || (a.Data != other.Data);

	}
}
