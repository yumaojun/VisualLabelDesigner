using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Barcodes
{
	/// <summary>
	/// Barcode Create 委托
	/// </summary>
	/// <returns></returns>
	public delegate Barcode BarcodeCreateFact();

	/// <summary>
	/// Barcode Factory
	/// </summary>
	public class Factory
	{
		private static Dictionary<string, BarcodeCreateFact> _barcodeTypeMap = new Dictionary<string, BarcodeCreateFact>();
		private static List<string> _supportedTypes = new List<string>();
		private static Factory singletonInstance = new Factory();

		/// <summary>
		/// Register built-in types.
		/// </summary>
		private Factory()
		{
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
			InternalRegisterType("qrencode::qrcode", BarcodeQrcode.Create);
		}

		public static Barcodes.Barcode CreateBarcode(string typeId)
		{
			if (_barcodeTypeMap.TryGetValue(typeId,  out var fct))
			{
				return fct();
			}

			return null;
		}

		private void InternalRegisterType(string typeId, BarcodeCreateFact fact)
		{
			_barcodeTypeMap[typeId] = fact;
			_supportedTypes.Add(typeId);
		}
	}
}
