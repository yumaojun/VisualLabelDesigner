using SkiaSharp;
using SkiaSharp.QrCode;
using SkiaSharp.QrCode.Image;
using System;
using System.Linq;

namespace YProgramStudio.LabelsDesigner.Barcodes
{
	/// <summary>
	/// Qrcode
	/// </summary>
	public class BarcodeQrcode : Barcode2dBase
	{
		public static Barcode Create()
		{
			return new BarcodeQrcode();
		}

		protected override bool Validate(string rawData)
		{
			if (rawData == null || rawData.Length == 0)
			{
				return false;
			}
			return true;
		}

		protected override bool Encode(string cookedData, Matrix<bool> encodedData)
		{
			if (Environment.Is64BitProcess) // x64
			{
				QREncode64.QRcode qrCode = QREncode64.qrencode.QRcode_encodeString(cookedData, 0, QREncode64.QRecLevel.QR_ECLEVEL_M, QREncode64.QRencodeMode.QR_MODE_8, 1);
				int w = qrCode.Width;
				encodedData.Resize(w, w);
				for (int iy = 0; iy < w; iy++)
				{
					for (int ix = 0; ix < w; ix++)
					{
						unsafe
						{
							encodedData[iy, ix] = (qrCode.Data[iy * w + ix] & 0x01) == 1 ? true : false;
						}
					}
				}
				qrCode.Dispose();
			}
			else
			{
				QREncode86.QRcode qrCode = QREncode86.qrencode.QRcode_encodeString(cookedData, 0, QREncode86.QRecLevel.QR_ECLEVEL_M, QREncode86.QRencodeMode.QR_MODE_8, 1);
				int w = qrCode.Width;
				encodedData.Resize(w, w);
				for (int iy = 0; iy < w; iy++)
				{
					for (int ix = 0; ix < w; ix++)
					{
						unsafe
						{
							encodedData[iy, ix] = (qrCode.Data[iy * w + ix] & 0x01) == 1 ? true : false;
						}
					}
				}
				qrCode.Dispose();
			}

			return true;
		}
	}
}