// Copyright (c) 2023 余茂军 yumaojun@gmail.com

namespace YProgramStudio.LabelsDesigner.Barcodes
{
	/// <summary>
	/// EAN-13 barcode, implements BarcodeUpcBase EAN-13条形码，实现抽象基类BarcodeUpcBase
	/// </summary>
	public class BarcodeEan13 : BarcodeUpcBase
	{
		/// <summary>
		/// Static EAN-13 barcode creation method 静态EAN-13条形码创建方法
		/// </summary>
		/// <returns></returns>
		public static Barcode Create()
		{
			return new BarcodeEan13();
		}

		/// <summary>
		/// EAN-13 barcode constructor EAN-13条形码默认构造函数
		/// </summary>
		public BarcodeEan13()
		{
			_endBarsModules = 3;
		}

		/// <summary>
		/// EAN-13 validate number of digits, implements BarcodeUpcBase::validateDigits() EAN-13验证位数，实现BarcodeUpcBase::validateDigital()
		/// </summary>
		/// <param name="nDigits"></param>
		/// <returns></returns>
		protected override bool ValidateDigits(int nDigits)
		{
			return nDigits == 12;
		}

		/// <summary>
		/// EAN-13 Pre-process data before encoding, implements Barcode1dBase::preprocess() EAN-13编码前预处理数据，实现Barcode1dBase::preprocess()
		/// </summary>
		/// <param name="rawData"></param>
		/// <returns></returns>
		protected override string Preprocess(string rawData)
		{
			string cookedData = string.Empty;

			foreach (char c in rawData)
			{
				if (char.IsDigit(c))
				{
					cookedData += c;
				}
			}

			_firstDigitVal = cookedData[0] - '0';
			return cookedData.Substring(1, cookedData.Length - 1);
		}

		/// <summary>
		/// EAN-13 vectorize text, implements BarcodeUpcBase::vectorizeText() EAN-13矢量化文本，实现BarcodeUpcBase::vectorizeText()
		/// </summary>
		/// <param name="displayText"></param>
		/// <param name="size1"></param>
		/// <param name="size2"></param>
		/// <param name="x1Left"></param>
		/// <param name="x1Right"></param>
		/// <param name="y1"></param>
		/// <param name="x2Left"></param>
		/// <param name="x2Right"></param>
		/// <param name="y2"></param>
		protected override void VectorizeText(string displayText, float size1, float size2, float x1Left, float x1Right, float y1, float x2Left, float x2Right, float y2)
		{
			AddText(x2Left, y2, size2, displayText.Substring(0, 1));
			AddText(x1Left, y1, size1, displayText.Substring(1, 6));
			AddText(x1Right, y1, size1, displayText.Substring(7, 6));
		}
	}
}