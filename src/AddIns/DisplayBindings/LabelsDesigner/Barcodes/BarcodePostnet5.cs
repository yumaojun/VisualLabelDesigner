namespace YProgramStudio.LabelsDesigner.Barcodes
{
	/// <summary>
	/// *POSTNET-5* barcode (ZIP only), extends BarcodePostnet
	/// </summary>
	public class BarcodePostnet5 : BarcodePostnet
	{
		/// <summary>
		/// Static POSTNET-5 barcode creation method
		/// </summary>
		public static new Barcode Create()
		{
			return new BarcodePostnet5();
		}

		protected override bool ValidateDigits(int nDigits)
		{
			return nDigits == 5; /* Zip only */
		}
	}
}