// Copyright (c) 2023 余茂军 <yumaojun@gmail.com> All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YProgramStudio.LabelsDesigner.Model;

namespace YProgramStudio.LabelsDesigner.Barcodes
{
	/// <summary>
	/// Parity selection
	/// </summary>
	public enum Parity { ODD, EVEN };

	/// <summary>
	/// UpcBase barcode, base class for UPC-A and EAN-13 barcode types, implements Barcode1dBase
	/// </summary>
	public abstract class BarcodeUpcBase : Barcode1dBase
	{
		const int QUIET_MODULES = 9;

		const float BASE_MODULE_SIZE = 0.01f * Constants.PTS_PER_INCH;
		const float BASE_FONT_SIZE = 7;
		const float BASE_TEXT_AREA_HEIGHT = 11;

		static readonly Parity[,] parity = new Parity[10, 6]{
			/*                Pos 1,  Pos 2,  Pos 3,  Pos 4,  Pos 5,  Pos 6 */
			/* 0 (UPC-A) */ { Parity.ODD,  Parity.ODD,  Parity.ODD,  Parity.ODD,  Parity.ODD,  Parity.ODD  },
			/* 1         */ { Parity.ODD,  Parity.ODD,  Parity.EVEN, Parity.ODD,  Parity.EVEN, Parity.EVEN },
			/* 2         */ { Parity.ODD,  Parity.ODD,  Parity.EVEN, Parity.EVEN, Parity.ODD,  Parity.EVEN },
			/* 3         */ { Parity.ODD,  Parity.ODD,  Parity.EVEN, Parity.EVEN, Parity.EVEN, Parity.ODD  },
			/* 4         */ { Parity.ODD,  Parity.EVEN, Parity.ODD,  Parity.ODD,  Parity.EVEN, Parity.EVEN },
			/* 5         */ { Parity.ODD,  Parity.EVEN, Parity.EVEN, Parity.ODD,  Parity.ODD,  Parity.EVEN },
			/* 6         */ { Parity.ODD,  Parity.EVEN, Parity.EVEN, Parity.EVEN, Parity.ODD,  Parity.ODD  },
			/* 7         */ { Parity.ODD,  Parity.EVEN, Parity.ODD,  Parity.EVEN, Parity.ODD,  Parity.EVEN },
			/* 8         */ { Parity.ODD,  Parity.EVEN, Parity.ODD,  Parity.EVEN, Parity.EVEN, Parity.ODD  },
			/* 9         */ { Parity.ODD,  Parity.EVEN, Parity.EVEN, Parity.ODD,  Parity.EVEN, Parity.ODD  }
		};

		// Symbology
		static readonly string[,] symbols = new string[10, 2]{
			/*          Odd     Even  */
			/*    Left: sBsB    sBsB  */
			/*   Right: BsBs    ----  */
			/*                        */
			/* 0 */  { "3211", "1123" },
			/* 1 */  { "2221", "1222" },
			/* 2 */  { "2122", "2212" },
			/* 3 */  { "1411", "1141" },
			/* 4 */  { "1132", "2311" },
			/* 5 */  { "1231", "1321" },
			/* 6 */  { "1114", "4111" },
			/* 7 */  { "1312", "2131" },
			/* 8 */  { "1213", "3121" },
			/* 9 */  { "3112", "2113" }
		};

		const string sSymbol = "111";   /* BsB */
		const string eSymbol = "111";   /* BsB */
		const string mSymbol = "11111"; /* sBsBs */

		private int _checkDigitVal;

		protected int _endBarsModules;
		protected int _firstDigitVal;

		// 由子类实现，验证数字位数
		protected abstract bool ValidateDigits(int nDigits);

		// 由子类实现，矢量化文本
		protected abstract void VectorizeText(string displayText, float size1, float size2, float x1Left, float x1Right, float y1, float x2Left, float x2Right, float y2);

		// 验证
		protected override bool Validate(string rawData)
		{
			int nDigits = 0;

			foreach (char c in rawData)
			{
				if (char.IsDigit(c))
				{
					nDigits++;
				}
				else if (c != ' ')
				{
					/* Only allow digits and spaces -- ignoring spaces. */
					return false;
				}
			}

			/* validate nDigits (call implementation from concrete class) */
			return ValidateDigits(nDigits);
		}

		/// <summary>
		/// UPC data encoding, implements Barcode1dBase::encode()
		/// </summary>
		protected override string Encode(string cookedData)
		{
			int sumOdd = 0;
			int sumEven = _firstDigitVal;

			string code = string.Empty;

			/* Left frame symbol */
			code += sSymbol;

			/* Left 6 digits */
			for (int i = 0; i < 6; i++)
			{
				int cValue = cookedData[i] - '0';
				code += symbols[cValue, (int)parity[_firstDigitVal, i]];

				if ((i & 1) == 0)
				{
					sumOdd += cValue;
				}
				else
				{
					sumEven += cValue;
				}
			}

			/* Middle frame symbol */
			code += mSymbol;

			/* Right 5 digits */
			for (int i = 6; i < 11; i++)
			{
				int cValue = cookedData[i] - '0';
				code += symbols[cValue, (int)Parity.ODD];

				if ((i & 1) == 0)
				{
					sumOdd += cValue;
				}
				else
				{
					sumEven += cValue;
				}
			}

			/* Check digit */
			_checkDigitVal = (3 * sumOdd + sumEven) % 10;
			if (_checkDigitVal != 0)
			{
				_checkDigitVal = 10 - _checkDigitVal;
			}
			code += symbols[_checkDigitVal, (int)Parity.ODD];

			/* Right frame symbol */
			code += eSymbol;

			/* Append a final zero length space to make the length of the encoded string even. */
			/* This is because vectorize() handles bars and spaces in pairs. */
			code += "0";

			return code;
		}

		protected override string PrepareText(string rawData)
		{
			string displayText = string.Empty;

			if (ShowText)
			{
				foreach (char c in rawData)
				{
					if (char.IsDigit(c))
					{
						displayText += c;
					}
				}

				displayText += (char)(_checkDigitVal + '0');
			}

			return displayText;
		}

		protected override void Vectorize(string codedData, string displayText, string cookedData, ref float w, ref float h)
		{
			/* determine width and establish horizontal scale */
			int nModules = 7 * (cookedData.Length + 1) + 11;

			float scale;

			if (w == 0)
			{
				scale = 1.0f;
			}
			else
			{
				scale = w / ((nModules + 2 * QUIET_MODULES) * BASE_MODULE_SIZE);

				if (scale < 1.0f)
				{
					scale = 1.0f;
				}
			}

			float mscale = scale * BASE_MODULE_SIZE;
			float width = mscale * (nModules + 2 * QUIET_MODULES);
			float xQuiet = mscale * QUIET_MODULES;

			/* determine bar height */
			float hTextArea = ShowText ? scale * BASE_TEXT_AREA_HEIGHT : 0f;
			float hBar1 = Math.Max(h - hTextArea, width / 2);
			float hBar2 = hBar1 + hTextArea / 2;

			/* now traverse the code string and draw each bar */
			int nBarsSpaces = codedData.Length - 1; /* coded data has dummy "0" on end. */

			float xModules = 0;
			for (int i = 0; i < nBarsSpaces; i += 2)
			{
				float hBar;

				if (((xModules > _endBarsModules) && (xModules < (nModules / 2 - 1))) ||
					 ((xModules > (nModules / 2 + 1)) && (xModules < (nModules - _endBarsModules))))
				{
					hBar = hBar1;
				}
				else
				{
					hBar = hBar2;
				}

				/* Bar */
				int wBar = codedData[i] - '0';
				AddLine(xQuiet + mscale * xModules, 0.0f, mscale * wBar, hBar);
				xModules += wBar;

				/* Space */
				int wSpace = codedData[i + 1] - '0';
				xModules += wSpace;
			}

			/* draw text */
			if (ShowText)
			{
				/* determine text parameters */
				float textSize1 = scale * BASE_FONT_SIZE;
				float textSize2 = 0.75f * textSize1;

				float textX1Left = xQuiet + mscale * (0.25f * nModules + 0.5f * _endBarsModules - 0.75f);
				float textX1Right = xQuiet + mscale * (0.75f * nModules - 0.5f * _endBarsModules + 0.75f);
				float textX2Left = 0.5f * xQuiet;
				float textX2Right = 1.5f * xQuiet + mscale * nModules;

				float textY1 = hBar2 + textSize1 / 4;
				float textY2 = hBar2 + textSize2 / 4;

				/* draw text (call implementation from concrete class) */
				VectorizeText(displayText, textSize1, textSize2, textX1Left, textX1Right, textY1, textX2Left, textX2Right, textY2);
			}

			/* Overwrite requested size with actual size. */
			w = width;
			h = hBar1 + hTextArea;
		}
	}
}
