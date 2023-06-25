using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Barcodes
{
	/// <summary>
	/// 一维条码基类
	/// </summary>
	public class Barcode1dBase : Barcode
	{
		public override Barcode Build(string rawData, float w, float h)
		{
			string cookedData;     /* Preprocessed data */
			string displayText;    /* Text data to be displayed */
			string codedData;      /* Encoded data */

			Clear();

			if (string.IsNullOrEmpty(rawData))
			{
				IsEmpty = true;
				IsDataValid = false;

				Width = 0;
				Height = 0;
			}
			else
			{
				IsEmpty = false;

				if (!Validate(rawData))
				{
					IsDataValid = false;

					Width = 0;
					Height = 0;
				}
				else
				{
					IsDataValid = true;

					cookedData = Preprocess(rawData);
					codedData = Encode(cookedData);
					displayText = PrepareText(rawData);

					Vectorize(codedData, displayText, cookedData, ref w, ref h);

					Width = w;
					Height = h;
				}
			}

			return this;
		}

		protected virtual bool Validate(string rawData)
		{
			return false;
		}

		protected virtual string Preprocess(string rawData)
		{
			return rawData;
		}

		protected virtual string Encode(string cookedData)
		{
			return string.Empty;
		}

		protected virtual string PrepareText(string rawData)
		{
			return rawData;
		}

		protected virtual void Vectorize(string encodedData, string displayText, string cookedData, ref float w, ref float h)
		{
		}
	}
}
