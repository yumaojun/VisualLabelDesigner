namespace YProgramStudio.LabelsDesigner.Barcodes
{
	/// <summary>
	/// *POSTNET-9* barcode (ZIP+4 only), extends BarcodePostnet
	/// </summary>
	public class BarcodePostnet9 : BarcodePostnet
	{
		/// <summary>
		/// Static POSTNET-9 barcode creation method
		/// </summary>
		public static new Barcode Create()
		{
			return new BarcodePostnet9();
		}

		/// <summary>
		/// Postnet-9 validation of number of digits, overrides BarcodePostnet::validateDigits()
		/// </summary>
		protected override bool ValidateDigits(int nDigits)
		{
			return nDigits == 9; /* Zip + 4 */
		}
	}
}