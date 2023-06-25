namespace YProgramStudio.LabelsDesigner.Barcodes
{
	/// <summary>
	/// EAN-13 barcode, implements BarcodeUpcBase
	/// </summary>
	public class BarcodeEan13 : BarcodeUpcBase
	{
		public static Barcode Create()
		{
			return new BarcodeEan13();
		}

		protected override bool ValidateDigits(int nDigits)
		{
			return nDigits == 12;
		}

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

			_firstDigitVal = cookedData[0] - '0';
			return cookedData.Substring(1, cookedData.Length - 1);
		}

		protected override void VectorizeText(string displayText, float size1, float size2, float x1Left, float x1Right, float y1, float x2Left, float x2Right, float y2)
		{
			AddText(x2Left, y2, size2, displayText.Substring(0, 1));
			AddText(x1Left, y1, size1, displayText.Substring(1, 6));
			AddText(x1Right, y1, size1, displayText.Substring(7, 6));
		}
	}
}