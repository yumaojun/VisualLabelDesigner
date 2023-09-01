// Copyright (c) 2023 余茂军 <yumaojun@gmail.com> All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Barcodes
{
	/// <summary>
	/// The Barcode1dBase class is the base class for all 1D barcode implementations.
	/// This class provides a common framework for the implementation of 1D barcodes.
	/// Creating 1D barcode types(or symbologies) would be typically accomplished by
	/// implementing this class rather than directly implementing the Barcode class.
	/// </summary>
	public abstract class Barcode1dBase : Barcode
	{
		/// <summary>
		/// Build 1D barcode from data.
		/// Implements Barcode::build().  Calls the validate(), preprocess(),
		/// encode(), prepareText(), and vectorize() virtual methods, as shown: figure-1d-build.dot "1D build() data flow"
		/// </summary>
		/// <param name="rawData"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		/// <returns></returns>
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

		// 由子类实现，验证数据是否OK
		protected abstract bool Validate(string rawData);

		// 由子类实现，编码
		protected abstract string Encode(string cookedData);

		// 由子类实现，矢量化
		protected abstract void Vectorize(string encodedData, string displayText, string cookedData, ref float w, ref float h);

		/// <summary>
		/// Default preprocess method 默认预处理方法
		/// </summary>
		/// <param name="rawData"></param>
		/// <returns></returns>
		protected virtual string Preprocess(string rawData)
		{
			return rawData;
		}

		/// <summary>
		/// Default prepareText method 默认prepareText方法
		/// </summary>
		/// <param name="rawData"></param>
		/// <returns></returns>
		protected virtual string PrepareText(string rawData)
		{
			return rawData;
		}
	}
}
