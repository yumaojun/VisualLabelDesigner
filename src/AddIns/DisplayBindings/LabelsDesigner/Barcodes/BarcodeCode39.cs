using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YProgramStudio.LabelsDesigner.Model;

namespace YProgramStudio.LabelsDesigner.Barcodes
{
	/// <summary>
	/// Code39
	/// </summary>
	public class BarcodeCode39 : Barcode1dBase
	{
		/* Code 39 alphabet. Position indicates value. */
		private const string alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%";

		/* Code 39 symbols. Position must match position in alphabet. */
		private static string[] symbols = new string[]{
			/*        BsBsBsBsB */
			/* 0 */  "NnNwWnWnN",
			/* 1 */  "WnNwNnNnW",
			/* 2 */  "NnWwNnNnW",
			/* 3 */  "WnWwNnNnN",
			/* 4 */  "NnNwWnNnW",
			/* 5 */  "WnNwWnNnN",
			/* 6 */  "NnWwWnNnN",
			/* 7 */  "NnNwNnWnW",
			/* 8 */  "WnNwNnWnN",
			/* 9 */  "NnWwNnWnN",
			/* A */  "WnNnNwNnW",
			/* B */  "NnWnNwNnW",
			/* C */  "WnWnNwNnN",
			/* D */  "NnNnWwNnW",
			/* E */  "WnNnWwNnN",
			/* F */  "NnWnWwNnN",
			/* G */  "NnNnNwWnW",
			/* H */  "WnNnNwWnN",
			/* I */  "NnWnNwWnN",
			/* J */  "NnNnWwWnN",
			/* K */  "WnNnNnNwW",
			/* L */  "NnWnNnNwW",
			/* M */  "WnWnNnNwN",
			/* N */  "NnNnWnNwW",
			/* O */  "WnNnWnNwN",
			/* P */  "NnWnWnNwN",
			/* Q */  "NnNnNnWwW",
			/* R */  "WnNnNnWwN",
			/* S */  "NnWnNnWwN",
			/* T */  "NnNnWnWwN",
			/* U */  "WwNnNnNnW",
			/* V */  "NwWnNnNnW",
			/* W */  "WwWnNnNnN",
			/* X */  "NwNnWnNnW",
			/* Y */  "WwNnWnNnN",
			/* Z */  "NwWnWnNnN",
			/* - */  "NwNnNnWnW",
			/* . */  "WwNnNnWnN",
			/*   */  "NwWnNnWnN",
			/* $ */  "NwNwNwNnN",
			/* / */  "NwNwNnNwN",
			/* + */  "NwNnNwNwN",
			/* % */  "NnNwNwNwN"
		};

		private const string frameSymbol = "NwNnWnWnN";

		/* Vectorization constants */
		private const float MIN_X = (0.0075f * Constants.PTS_PER_INCH);
		private const float N = 2.5f;
		private const float MIN_I = MIN_X;
		private const float MIN_HEIGHT = (0.19685f * Constants.PTS_PER_INCH);
		private const float MIN_QUIET = (10 * MIN_X);

		private const float MIN_TEXT_AREA_HEIGHT = 12.0f;
		private const float MIN_TEXT_SIZE = 8.0f;

		/**
		 * Static Code39 barcode creation method
		 *
		 * Used by glbarcode::BarcodeFactory
		 */
		public static Barcode Create()
		{
			return new BarcodeCode39();
		}

		protected override bool Validate(string rawData)
		{
			return rawData.Any(r => alphabet.Contains(char.ToUpper(r)));
		}

		protected override string Encode(string cookedData)
		{
			string code = string.Empty;

			/* Left frame symbol */
			code += frameSymbol;
			code += "i";

			int sum = 0;
			foreach (char c in cookedData)
			{
				int cValue = alphabet.IndexOf(char.ToUpper(c));

				code += symbols[cValue];
				code += "i";

				sum += cValue;
			}

			if (Checksum)
			{
				code += symbols[sum % 43];
				code += "i";
			}

			/* Right frame bar */
			code += frameSymbol;

			return code;
		}

		protected override string PrepareText(string rawData) {
			return rawData.ToUpper();
		}

		protected override void Vectorize(string codedData, string displayText, string cookedData, ref float w, ref float h) {
			/* determine width and establish horizontal scale, based on original cooked data */
			int dataSize = cookedData.Length;
			float minL;

			if (!Checksum)
			{
				minL = (dataSize + 2) * (3 * N + 6) * MIN_X + (dataSize + 1) * MIN_I;
			}
			else
			{
				minL = (dataSize + 3) * (3 * N + 6) * MIN_X + (dataSize + 2) * MIN_I;
			}

			float scale;
			if (w == 0)
			{
				scale = 1.0f;
			}
			else
			{
				scale = w / (minL + 2 * MIN_QUIET);

				if (scale < 1.0f)
				{
					scale = 1.0f;
				}
			}
			float width = minL * scale;

			/* determine text parameters */
			float hTextArea = scale * MIN_TEXT_AREA_HEIGHT;
			float textSize = scale * MIN_TEXT_SIZE;

			/* determine height of barcode */
			float height = ShowText ? h - hTextArea : h;
			height = Math.Max(height, Math.Max(0.15f * width, MIN_HEIGHT));

			/* determine horizontal quiet zone */
			float xQuiet = Math.Max((10 * scale * MIN_X), MIN_QUIET);

			/* Now traverse the code string and draw each bar */
			float x1 = xQuiet;
			foreach (char c in codedData)
			{
				float lwidth;

				switch (c)
				{
					case 'i':
						/* Inter-character gap */
						x1 += scale * MIN_I;
						break;

					case 'N':
						/* Narrow bar */
						lwidth = scale * MIN_X;
						AddLine(x1, 0.0f, lwidth, height);
						x1 += scale * MIN_X;
						break;

					case 'W':
						/* Wide bar */
						lwidth = scale * N * MIN_X;
						AddLine(x1, 0.0f, lwidth, height);
						x1 += scale * N * MIN_X;
						break;

					case 'n':
						/* Narrow space */
						x1 += scale * MIN_X;
						break;

					case 'w':
						/* Wide space */
						x1 += scale * N * MIN_X;
						break;

					default:
						// NOT REACHED
						break;
				}
			}

			if (ShowText)
			{
				string starredText = "*" + displayText + "*";
				AddText(xQuiet + width / 2, height + (hTextArea + 0.7f * textSize) / 2, textSize, starredText);
			}

			/* Overwrite requested size with actual size. */
			w = width + 2 * xQuiet;
			h = ShowText ? height + hTextArea : height;
		}
	}
}
