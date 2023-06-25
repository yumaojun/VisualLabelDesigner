using YProgramStudio.LabelsDesigner.Model;

namespace YProgramStudio.LabelsDesigner.Barcodes
{
	public class BarcodeOnecode : Barcode1dBase
	{
		// Constants
		const float ONECODE_BAR_WIDTH = 0.02f * Constants.PTS_PER_INCH;
		const float ONECODE_FULL_HEIGHT = 0.145f * Constants.PTS_PER_INCH;
		const float ONECODE_ASCENDER_HEIGHT = 0.0965f * Constants.PTS_PER_INCH;
		const float ONECODE_DESCENDER_HEIGHT = 0.0965f * Constants.PTS_PER_INCH;
		const float ONECODE_TRACKER_HEIGHT = 0.048f * Constants.PTS_PER_INCH;
		const float ONECODE_FULL_OFFSET = 0f;
		const float ONECODE_ASCENDER_OFFSET = 0f;
		const float ONECODE_DESCENDER_OFFSET = 0.0485f * Constants.PTS_PER_INCH;
		const float ONECODE_TRACKER_OFFSET = 0.0485f * Constants.PTS_PER_INCH;
		const float ONECODE_BAR_PITCH = 0.0458f * Constants.PTS_PER_INCH;
		const float ONECODE_HORIZ_MARGIN = 0.125f * Constants.PTS_PER_INCH;
		const float ONECODE_VERT_MARGIN = 0.028f * Constants.PTS_PER_INCH;

		public static Barcode Create()
		{
			return new BarcodeOnecode();
		}

		protected override bool Validate(string rawData)
		{
			if (rawData.Length != 20 && rawData.Length != 25 && rawData.Length != 29 && rawData.Length != 31)
			{
				return false;
			}

			foreach (char c in rawData)
			{
				if (!char.IsDigit(c))
				{
					return false;
				}
			}

			if (rawData[1] > '4')
			{
				return false; /* Invalid Barcode Identifier. */
			}

			return true;
		}

		protected override string Encode(string cookedData)
		{
			return "";
		}

		/// <summary>
		/// Onecode vectorization, implements Barcode1dBase::vectorize()
		/// </summary>
		protected override void Vectorize(string codedData, string displayText, string cookedData, ref float w, ref float h)
		{
			float x = ONECODE_HORIZ_MARGIN;
			foreach (char c in codedData)
			{
				float y = ONECODE_VERT_MARGIN;
				float length = 0;

				switch (c)
				{
					case 'T':
						y += ONECODE_TRACKER_OFFSET;
						length = ONECODE_TRACKER_HEIGHT;
						break;
					case 'D':
						y += ONECODE_DESCENDER_OFFSET;
						length = ONECODE_DESCENDER_HEIGHT;
						break;
					case 'A':
						y += ONECODE_ASCENDER_OFFSET;
						length = ONECODE_ASCENDER_HEIGHT;
						break;
					case 'F':
						y += ONECODE_FULL_OFFSET;
						length = ONECODE_FULL_HEIGHT;
						break;
					default:
						// Not reached
						break;
				}

				float width = ONECODE_BAR_WIDTH;

				AddLine(x, y, width, length);

				x += ONECODE_BAR_PITCH;
			}

			/* Overwrite requested size with actual size. */
			w = x + ONECODE_HORIZ_MARGIN;
			h = ONECODE_FULL_HEIGHT + 2 * ONECODE_VERT_MARGIN;
		}

		private uint USPS_MSB_Math_CRC11GenerateFrameCheckSequence(byte[] ByteArrayPtr)
		{
			return 1;
		}
	}
}