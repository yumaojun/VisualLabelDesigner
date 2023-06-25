using System;
using YProgramStudio.LabelsDesigner.Model;

namespace YProgramStudio.LabelsDesigner.Barcodes
{
	/// <summary>
	/// 二维条码基类
	/// </summary>
	public class Barcode2dBase : Barcode
	{
		private const float MIN_CELL_SIZE = (1.0f / 64.0f * Constants.PTS_PER_INCH);

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

					Vectorize(encodedData,ref w, ref h);

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

		protected virtual bool Encode(string cookedData, Matrix<bool> encodedData)
		{
			return false;
		}

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
					if (encodedData[iy * ix])
					{
						AddBox(quietSize + ix * cellSize, quietSize + iy * cellSize, cellSize, cellSize);
					}
				}
			}

		}
	}
}