namespace YProgramStudio.LabelsDesigner.Barcodes
{
	/// <summary>
	/// *POSTNET-11* barcode (ZIP only), extends BarcodePostnet
	/// </summary>
	public class BarcodePostnet11 : BarcodePostnet
	{
		/// <summary>
		/// Static POSTNET-11 barcode creation method
		/// </summary>
		public static new Barcode Create()
		{
			return new BarcodePostnet11();
		}

		/// <summary>
		/// Postnet-11 validation of number of digits, overrides BarcodePostnet::validateDigits()
		/// </summary>
		protected override bool ValidateDigits(int nDigits)
		{
			return nDigits == 11; /* Zip + 4 + Delivery Code */
		}
	}
}