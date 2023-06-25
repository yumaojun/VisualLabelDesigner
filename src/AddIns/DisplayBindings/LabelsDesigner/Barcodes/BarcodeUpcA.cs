namespace YProgramStudio.LabelsDesigner.Barcodes
{
	/// <summary>
	/// UPC-A barcode, implements BarcodeUpcBase
	/// </summary>
	public class BarcodeUpcA : BarcodeUpcBase
	{
		public static Barcode Create()
		{
			return new BarcodeUpcA();
		}

		///	UPC-A validate number of digits, implements BarcodeUpcBase::validateDigits()
		protected override bool ValidateDigits(int nDigits)
		{
			return nDigits == 11;
		}

		/// UPC-A Pre-process data before encoding, implements Barcode1dBase::preprocess()
		protected override string Preprocess(string rawData)
		{
			string cookedData = string.Empty;

			foreach (char c in rawData)
			{
				if (char.IsDigit(c))
				{
					cookedData += c;
				}
			}

			_firstDigitVal = 0;
			return cookedData;
		}

		/// UPC-A vectorize text, implements BarcodeUpcBase::vectorizeText()
		protected override void VectorizeText(string displayText, float size1, float size2, float x1Left, float x1Right, float y1, float x2Left, float x2Right, float y2)
		{
			AddText(x2Left, y2, size2, displayText.Substring(0, 1));
			AddText(x1Left, y1, size1, displayText.Substring(1, 5));
			AddText(x1Right, y1, size1, displayText.Substring(6, 5));
			AddText(x2Right, y2, size2, displayText.Substring(11, 1));
		}
	}
}