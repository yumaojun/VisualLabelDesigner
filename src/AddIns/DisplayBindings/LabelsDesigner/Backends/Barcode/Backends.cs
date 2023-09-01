// Copyright (c) 2023 余茂军 <yumaojun@gmail.com> All rights reserved.

using System.Collections.Generic;
using System.Linq;
using YProgramStudio.LabelsDesigner.Model;

namespace YProgramStudio.LabelsDesigner.Backends.Barcode
{
	/// <summary>
	/// 条码后端服务
	/// </summary>
	public class Backends
	{
		// Static data
		private static Backends singletonInstance = null;
		private static Dictionary<string, string> _backendNameMap = new Dictionary<string, string>();
		private static List<string> _backendIdList = new List<string>();
		private static List<BarcodeStyle> _styleList = new List<BarcodeStyle>();

		public static Dictionary<string, string> BackendNameMap
		{
			get => _backendNameMap;
			set => _backendNameMap = value;
		}

		public static List<string> BackendIdList
		{
			get => _backendIdList;
			set => _backendIdList = value;
		}

		public static List<BarcodeStyle> StyleList
		{
			get => _styleList;
			set => _styleList = value;
		}

		private Backends()
		{
			RegisterStyle("code39", string.Empty, TranslateHelper.Tr("Code 39"), true, true, true, true, "1234567890", true, 10);
			RegisterStyle("code39ext", string.Empty, TranslateHelper.Tr("Code 39 Extended"), true, true, true, true, "1234567890", true, 10);
			RegisterStyle("upc-a", string.Empty, TranslateHelper.Tr("UPC-A"), true, true, true, false, "12345678901", false, 11);
			RegisterStyle("ean-13", string.Empty, TranslateHelper.Tr("EAN-13"), true, true, true, false, "123456789012", false, 12);
			RegisterStyle("postnet", string.Empty, TranslateHelper.Tr("POSTNET (any)"), false, false, true, false, "12345-6789-12", false, 11);
			RegisterStyle("postnet-5", string.Empty, TranslateHelper.Tr("POSTNET-5 (ZIP only)"), false, false, true, false, "12345", false, 5);
			RegisterStyle("postnet-9", string.Empty, TranslateHelper.Tr("POSTNET-9 (ZIP+4)"), false, false, true, false, "12345-6789", false, 9);
			RegisterStyle("postnet-11", string.Empty, TranslateHelper.Tr("POSTNET-11 (DPBC)"), false, false, true, false, "12345-6789-12", false, 11);
			RegisterStyle("cepnet", string.Empty, TranslateHelper.Tr("CEPNET"), false, false, true, false, "12345-678", false, 8);
			RegisterStyle("onecode", string.Empty, TranslateHelper.Tr("USPS Intelligent Mail"), false, false, true, false, "12345678901234567890", false, 20);
			RegisterStyle("datamatrix", string.Empty, TranslateHelper.Tr("IEC16022 (DataMatrix)"), false, false, true, false, "1234567890AB", false, 12);
			RegisterStyle("qrcode", "qrencode", TranslateHelper.Tr("IEC18004 (QRCode)"), false, false, true, false, "1234567890AB", false, 12); // backendid="qrencode"
		}

		public static void Init()
		{
			if (singletonInstance == null)
			{
				singletonInstance = new Backends();
			}
		}

		public static BarcodeStyle DefaultStyle()
		{
			return StyleList.First();
		}

		public static BarcodeStyle Style(string backendId, string styleId)
		{
			var bcStyle = StyleList.FirstOrDefault(x => x.BackendId == backendId && x.Id == styleId);
			return bcStyle ?? DefaultStyle();
		}

		public void RegisterBackend(string backendId, string backendName)
		{
			_backendIdList.Add(backendId);
			_backendNameMap[backendId] = backendName;
		}

		public void RegisterStyle(string id,
									  string backendId,
									  string name,
									  bool canText,
									  bool textOptional,
									  bool canChecksum,
									  bool checksumOptional,
									  string defaultDigits,
									  bool canFreeForm,
									  int preferedN)
		{
			BarcodeStyle style = new Barcode.BarcodeStyle(id, backendId, name,
						 canText, textOptional,
						 canChecksum, checksumOptional,
						 defaultDigits,
						 canFreeForm, preferedN);

			StyleList.Add(style);
		}
	}
}
