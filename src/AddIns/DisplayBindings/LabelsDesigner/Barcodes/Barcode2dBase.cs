// Copyright (c) 2023 余茂军 <yumaojun@gmail.com> All rights reserved.

using System;
using YProgramStudio.LabelsDesigner.Model;

namespace YProgramStudio.LabelsDesigner.Barcodes
{
	/// <summary>
	/// 二维条码基类
	/// </summary>
	public abstract class Barcode2dBase : Barcode
	{
		private const float MIN_CELL_SIZE = (1.0f / 64.0f * Constants.PTS_PER_INCH);

		/// <summary>
		/// Build 2D barcode from data.
		/// Implements Barcode::build().  Calls the validate(), preprocess(),
		/// encode(), and vectorize() virtual methods, as shown: "2D build() data flow"
		/// </summary>
		/// <param name="rawData"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		/// <returns></returns>
		public override Barcode Build(string rawData, float w = 0, float h = 0)
		{
			string cookedData = string.Empty;   /* Preprocessed data */
			Matrix<bool> encodedData = new Matrix<bool>();  /* Encoded data matrix */

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
					Encode(cookedData, encodedData);

					Vectorize(encodedData, ref w, ref h);

					Width = w;
					Height = h;
				}
			}

			return this;
		}

		// 由子类实现，验证数据
		protected abstract bool Validate(string rawData);

		// 由子类实现，编码
		protected abstract bool Encode(string cookedData, Matrix<bool> encodedData);

		/// <summary>
		/// 预处理数据
		/// </summary>
		/// <param name="rawData"></param>
		/// <returns></returns>
		protected virtual string Preprocess(string rawData)
		{
			return rawData;
		}

		/// <summary>
		/// 矢量化
		/// </summary>
		/// <param name="encodedData"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		protected virtual void Vectorize(Matrix<bool> encodedData, ref float w, ref float h)
		{
			/* determine size and establish scale */
			float scale;
			float minW = MIN_CELL_SIZE * encodedData.Nx + 2 * MIN_CELL_SIZE;
			float minH = MIN_CELL_SIZE * encodedData.Ny + 2 * MIN_CELL_SIZE;

			if ((w <= minW) || (h <= minH))
			{
				scale = 1;
				w = minW;
				h = minH;
			}
			else
			{
				scale = Math.Min(w / minW, h / minH);
				w = scale * minW;
				h = scale * minH;
			}

			float cellSize = scale * MIN_CELL_SIZE;
			float quietSize = scale * MIN_CELL_SIZE;

			for (int iy = 0; iy < encodedData.Ny; iy++)
			{
				for (int ix = 0; ix < encodedData.Nx; ix++)
				{
					if (encodedData[iy, ix])
					{
						AddBox(quietSize + ix * cellSize, quietSize + iy * cellSize, cellSize, cellSize);
					}
				}
			}
		}
	}
}