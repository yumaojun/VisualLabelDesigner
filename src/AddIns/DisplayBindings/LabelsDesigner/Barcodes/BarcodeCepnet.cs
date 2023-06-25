namespace YProgramStudio.LabelsDesigner.Barcodes
{
	/// <summary>
	/// 8 digit *CEPNET* barcode (Brazilian Post, based on POSTNET), extends BarcodePostnet
	/// </summary>
	public class BarcodeCepnet : BarcodePostnet
	{
		/// <summary>
		/// Static CEPNET barcode creation method
		/// </summary>
		public static new Barcode Create()
		{
			return new BarcodeCepnet();
		}

		/// <summary>
		/// CEPNET validation of number of digits, overrides BarcodePostnet::validateDigits()
		/// </summary>
		protected override bool ValidateDigits(int nDigits)
		{
			return nDigits == 8;
		}
	}
}