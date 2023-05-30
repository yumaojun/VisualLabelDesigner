using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	public enum VariableType
	{
		STRING,
		INTEGER,
		FLOATING_POINT,
		COLOR
	};

	public enum Increment
	{
		NEVER,
		PER_ITEM,
		PER_COPY,
		PER_PAGE
	};

	public class Variable
	{
		private VariableType _type;
		private string _name;
		private string _initialValue;
		private Increment _increment;
		private string _stepSize;

		private long _integerValue;
		private long _integerStep;
		private float _floatingPointValue;
		private float _floatingPointStep;

		public Variable() { }

		public Variable(VariableType type,

					   string name,
					   string initialValue,
					   Increment increment = Increment.NEVER,

					   string stepSize = "0")
		{
			_type = type;
			_name = name;
			_initialValue = initialValue;
			_increment = increment;
			_stepSize = stepSize;
		}

		public string Value()
		{
			switch (_type)
			{
				case VariableType.STRING:
					return _initialValue;
				case VariableType.INTEGER:
					return _integerValue.ToString();
				case VariableType.FLOATING_POINT:
					return _floatingPointValue.ToString("F15"); // TODO: *需要测试 QString::number(_floatingPointValue, 'g', 15 );
				case VariableType.COLOR:
					return _initialValue;
				default:
					return _initialValue;
			}
		}

		public static VariableType IdStringToType(string id)
		{
			if (id == "string")
			{
				return VariableType.STRING;
			}
			else if (id == "integer")
			{
				return VariableType.INTEGER;
			}
			else if (id == "float")
			{
				return VariableType.FLOATING_POINT;
			}
			if (id == "color")
			{
				return VariableType.COLOR;
			}
			else
			{
				return VariableType.STRING; // Default
			}
		}

		public static Increment IdStringToIncrement(string id)
		{
			if (id == "never")
			{
				return Increment.NEVER;
			}
			else if (id == "per_item")
			{
				return Increment.PER_ITEM;
			}
			else if (id == "per_copy")
			{
				return Increment.PER_COPY;
			}
			else if (id == "per_page")
			{
				return Increment.PER_PAGE;
			}
			else
			{
				return Increment.NEVER; // Default
			}
		}
	}
}
