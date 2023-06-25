using YProgramStudio.LabelsDesigner.Model;

namespace YProgramStudio.LabelsDesigner.Barcodes
{
	/// <summary>
	/// *POSTNET* barcode (All USPS sizes: ZIP, ZIP+4, ZIP+4+DC).
	/// </summary>
	public class BarcodePostnet : Barcode1dBase
	{
		// Encoding symbology
		static readonly string[] symbols = {
			/* 0 */ "11000",
			/* 1 */ "00011",
			/* 2 */ "00101",
			/* 3 */ "00110",
			/* 4 */ "01001",
			/* 5 */ "01010",
			/* 6 */ "01100",
			/* 7 */ "10001",
			/* 8 */ "10010",
			/* 9 */ "10100"
		};

		const string frameSymbol = "1";

		// Constants
		const float POSTNET_BAR_WIDTH = 0.02f * Constants.PTS_PER_INCH;
		const float POSTNET_FULLBAR_HEIGHT = 0.125f * Constants.PTS_PER_INCH;
		const float POSTNET_HALFBAR_HEIGHT = 0.05f * Constants.PTS_PER_INCH;
		const float POSTNET_BAR_PITCH = 0.04545f * Constants.PTS_PER_INCH;
		const float POSTNET_HORIZ_MARGIN = 0.125f * Constants.PTS_PER_INCH;
		const float POSTNET_VERT_MARGIN = 0.04f * Constants.PTS_PER_INCH;

		/// <summary>
		/// Static Postnet barcode creation method
		/// </summary>
		public static Barcode Create()
		{
			return new BarcodePostnet();
		}

		/// Postnet validate number of digits
		protected virtual bool ValidateDigits(int nDigits)
		{
			/* Accept any valid USPS POSTNET length. */
			return (nDigits == 5) || (nDigits == 9) || (nDigits == 11);
		}

		protected override bool Validate(string rawData)
		{
			int nDigits = 0;

			foreach (char c in rawData)
			{
				if (char.IsDigit(c))
				{
					nDigits++;
				}
				else if ((c != '-') && (c != ' '))
				{
					/* Only allow digits, dashes, and spaces. */
					return false;
				}
			}

			return ValidateDigits(nDigits);
		}

		protected override string Encode(string cookedData)
		{
			string code = string.Empty;

			/* Left frame bar */
			code += frameSymbol;

			/* process each digit, adding appropriate symbol */
			int sum = 0;
			foreach (char c in cookedData)
			{
				if (char.IsDigit(c))
				{
					/* Only translate the digits (0-9) */
					int d = c - '0';
					code += symbols[d];
					sum += d;
				}
			}

			/* Create mandatory correction character */
			code += symbols[(10 - (sum % 10)) % 10];

			/* Right frame bar */
			code += frameSymbol;

			return code;
		}

		protected override void Vectorize(string codedData, string displayText, string cookedData, ref float w, ref float h)
		{
			float x = POSTNET_HORIZ_MARGIN;
			foreach (char c in codedData)
			{
				float y = POSTNET_VERT_MARGIN;

				float length = 0;
				switch (c)
				{
					case '0':
						y += POSTNET_FULLBAR_HEIGHT - POSTNET_HALFBAR_HEIGHT;
						length = POSTNET_HALFBAR_HEIGHT;
						break;
					case '1':
						length = POSTNET_FULLBAR_HEIGHT;
						break;
					default:
						// Not reached
						break;
				}

				float width = POSTNET_BAR_WIDTH;

				AddLine(x, y, width, length);

				x += POSTNET_BAR_PITCH;
			}

			/* Overwrite requested size with actual size. */
			w = x + POSTNET_HORIZ_MARGIN;
			h = POSTNET_FULLBAR_HEIGHT + 2 * POSTNET_VERT_MARGIN;
		}
	}
}