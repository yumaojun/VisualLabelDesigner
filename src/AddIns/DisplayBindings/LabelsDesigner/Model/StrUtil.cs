using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	public class StrUtil
	{
		static readonly double FRAC_EPSILON = 0.00005;
		static readonly double[] denom = { 1.0, 2.0, 3.0, 4.0, 8.0, 16.0, 32.0, 0.0 };
		static readonly string[] denom_string = { "1", "₂", "₃", "₄", "₈", "₁₆", "₃₂", null };
		static readonly string[] num_string = {  "⁰",  "¹",  "²",  "³",  "⁴",  "⁵",  "⁶",  "⁷",  "⁸",  "⁹",
											 "¹⁰", "¹¹", "¹²", "¹³", "¹⁴", "¹⁵", "¹⁶", "¹⁷", "¹⁸", "¹⁹",
											 "²⁰", "²¹", "²²", "²³", "²⁴", "²⁵", "²⁶", "²⁷", "²⁸", "²⁹",
											 "³⁰", "³¹" };

		public static string FormatFraction(double x)
		{
			int i;
			double product, remainder;

			for (i = 0; denom[i] != 0.0; i++)
			{
				product = x * denom[i];
				remainder = Math.Abs(product - ((int)(product + 0.5)));
				if (remainder < FRAC_EPSILON) break;
			}

			if (denom[i] == 0.0)
			{
				/* None of our denominators work. */
				return $"{x:D3}"; // %.3f
			}
			if (denom[i] == 1.0)
			{
				/* Simple integer. */
				return $"{x}"; // %.0f
			}
			var n = (int)(x * denom[i] + 0.5);
			var d = (int)(denom[i]);
			if (n > d)
			{
				return (n / d).ToString() + num_string[n % d] + "/" + denom_string[i];
			}
			else
			{
				return num_string[n % d] + "/" + denom_string[i];
			}
		}
	}
}
