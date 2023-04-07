using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualLabelDesigner.ZplTextEditor
{
	public class Measure
	{
		public double Value { get; set; }

		public Unit Unit { get; set; }

		public double ToInch()
		{
			double result;
			try
			{
				string name = this.Unit.Name;
				if (!(name == "mm"))
				{
					if (!(name == "cm"))
					{
						if (!(name == "in"))
						{
							result = 0.0;
						}
						else
						{
							result = this.Value;
						}
					}
					else
					{
						result = Math.Round(this.Value * 0.393701, 3);
					}
				}
				else
				{
					result = Math.Round(this.Value * 0.0393701, 3);
				}
			}
			catch (Exception)
			{
				result = 0.0;
			}
			return result;
		}

		public double ToCentimeter()
		{
			double result;
			try
			{
				string name = this.Unit.Name;
				if (!(name == "mm"))
				{
					if (!(name == "cm"))
					{
						if (!(name == "in"))
						{
							result = 0.0;
						}
						else
						{
							result = Math.Round(this.Value * 2.54, 3);
						}
					}
					else
					{
						result = this.Value;
					}
				}
				else
				{
					result = this.Value / 10.0;
				}
			}
			catch (Exception)
			{
				result = 0.0;
			}
			return result;
		}

		public double ToMillimeter()
		{
			double result;
			try
			{
				string name = this.Unit.Name;
				if (!(name == "mm"))
				{
					if (!(name == "cm"))
					{
						if (!(name == "in"))
						{
							result = 0.0;
						}
						else
						{
							result = Math.Round(this.Value * 22.679, 0);
						}
					}
					else
					{
						result = this.Value * 10.0;
					}
				}
				else
				{
					result = this.Value;
				}
			}
			catch (Exception)
			{
				result = 0.0;
			}
			return result;
		}
	}
}
