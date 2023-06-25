using SkiaSharp;
using SkiaSharp.QrCode;
using SkiaSharp.QrCode.Image;
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

			// QRcode qrcode = QRcode_encodeString(cookedData, 0, QR_ECLEVEL_M, QR_MODE_8, 1);
			using (var generator = new QRCodeGenerator())
			{
				QRCodeData qr = generator.CreateQrCode(cookedData, ECCLevel.M);
				var data = qr.GetRawData(QRCodeData.Compression.Uncompressed);
				var matrix = qr.ModuleMatrix;
				var one = matrix.First();


				if (qr == null)
				{
					return false;
				}
				//var qrCode = new QrCode("", new Vector2Slim(256, 256), SKEncodedImageFormat.Png);
				//int w = qrCode.Width;
				//encodedData.Resize(w, w);

				//for (int iy = 0; iy < w; iy++)
				//{
				//	for (int ix = 0; ix < w; ix++)
				//	{
				//		encodedData[iy * ix] = qrcode->data[iy * w + ix] & 0x01;
				//	}
				//}



			}


			//QRcode_free(qrcode);
			//QRcode_clearCache();

			return true;
		}
	}
}