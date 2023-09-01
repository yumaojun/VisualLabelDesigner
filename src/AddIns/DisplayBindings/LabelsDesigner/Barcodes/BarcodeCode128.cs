using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Barcodes
{
	/// <summary>
	/// BarcodeCode 128
	/// </summary>
	public class BarcodeCode128 : Barcode1dBase
	{
		/**
		 * Static Code39 barcode creation method
		 *
		 * Used by glbarcode::BarcodeFactory
		 */
		public static Barcode Create()
		{
			return new BarcodeCode128();
		}

		protected override string Encode(string cookedData)
		{
			throw new NotImplementedException();
		}

		protected override bool Validate(string rawData)
		{
			throw new NotImplementedException();
		}

		protected override void Vectorize(string encodedData, string displayText, string cookedData, ref float w, ref float h)
		{
			throw new NotImplementedException();
		}
	}
}
