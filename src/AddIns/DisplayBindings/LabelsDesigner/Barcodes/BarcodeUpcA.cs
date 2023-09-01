// Copyright (c) 2023 余茂军 <yumaojun@gmail.com> All rights reserved.

namespace YProgramStudio.LabelsDesigner.Barcodes
{
	/// <summary>
	/// UPC-A barcode, implements BarcodeUpcBase UPC-A条形码，实现抽象基类BarcodeUpcBase
	/// </summary>
	public class BarcodeUpcA : BarcodeUpcBase
	{
		/// <summary>
		/// Static UPC-A barcode creation method UPC-A条形码静态工厂方法
		/// </summary>
		/// <returns></returns>
		public static Barcode Create()
		{
			return new BarcodeUpcA();
		}

		/// <summary>
		/// UPC-A barcode constructor UPC-A条形码默认构造函数
		/// </summary>
		public BarcodeUpcA()
		{
			_endBarsModules = 7;
		}

		/// <summary>
		/// UPC-A validate number of digits, implements BarcodeUpcBase::validateDigits() UPC-A验证位数，实现BarcodeUpcBase::validateDigital()
		/// </summary>
		/// <param name="nDigits"></param>
		/// <returns></returns>
		protected override bool ValidateDigits(int nDigits)
		{
			return nDigits == 11;
		}

		/// <summary>
		/// UPC-A Pre-process data before encoding, implements Barcode1dBase::preprocess() UPC-A在编码前预处理数据，实现Barcode1dBase::preprocess()
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

			_firstDigitVal = 0;
			return cookedData;
		}

		/// <summary>
		///  UPC-A vectorize text, implements BarcodeUpcBase::vectorizeText() UPC-A矢量化文本，实现BarcodeUpcBase::vectorizeText() 
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
			AddText(x1Left, y1, size1, displayText.Substring(1, 5));
			AddText(x1Right, y1, size1, displayText.Substring(6, 5));
			AddText(x2Right, y2, size2, displayText.Substring(11, 1));
		}
	}
}