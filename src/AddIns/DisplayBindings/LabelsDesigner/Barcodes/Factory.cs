using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Barcodes
{
	/// <summary>
	/// Barcode Create委托
	/// </summary>
	/// <returns></returns>
	public delegate Barcode BarcodeCreateFct();

	/// <summary>
	/// Barcode Factory
	/// </summary>
	public class Factory
	{
		private static Factory singletonInstance = null;
		private static Dictionary<string, BarcodeCreateFct> _barcodeTypeMap = new Dictionary<string, BarcodeCreateFct>();
		private static List<string> _supportedTypes = new List<string>();

		private Factory()
		{
			/*
			 * Register built-in types.
			 */
			InternalRegisterType("code39", BarcodeCode39.Create);
			InternalRegisterType("code39ext", BarcodeCode39Ext.Create);
			InternalRegisterType("upc-a", BarcodeUpcA.Create);
			InternalRegisterType("ean-13", BarcodeEan13.Create);
			InternalRegisterType("postnet", BarcodePostnet.Create);
			InternalRegisterType("postnet-5", BarcodePostnet5.Create);
			InternalRegisterType("postnet-9", BarcodePostnet9.Create);
			InternalRegisterType("postnet-11",BarcodePostnet11.Create);
			InternalRegisterType("cepnet", BarcodeCepnet.Create);
			InternalRegisterType("onecode", BarcodeOnecode.Create);
			InternalRegisterType("datamatrix", BarcodeDataMatrix.Create);
			// HAVE_QRENCODE
			InternalRegisterType("qrcode", BarcodeQrcode.Create);

		}

		public static void Init()
		{
			if (singletonInstance == null)
			{
				singletonInstance = new Factory();
			}
		}

		public static Barcodes.Barcode CreateBarcode(string typeId)
		{
			Barcodes.BarcodeCreateFct fct;
			bool fctOk = _barcodeTypeMap.TryGetValue(typeId, out fct);

			if (fctOk)
			{
				return fct();
			}

			return null;
		}

		private void InternalRegisterType(string typeId, BarcodeCreateFct fct)
		{
			_barcodeTypeMap[typeId] = fct;
			_supportedTypes.Add(typeId);
		}
	}
}
