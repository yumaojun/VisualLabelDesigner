using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Barcodes
{
	/// <summary>
	/// 条码数据
	/// </summary>
	public struct BarcodeData
	{
		public BarcodeData(bool checksum, bool showText, float width, float height, bool isEmpty, bool isDataValid)
		{
			ChecksumFlag = checksum;
			ShowTextFlag = showText;
			Width = width;
			Height = height;
			IsEmpty = isEmpty;
			IsDataValid = isDataValid;
			Primitives = new List<DrawingPrimitive>();
		}

		public bool ChecksumFlag;  /**< Add checksum flag */
		public bool ShowTextFlag;  /**< Display text flag */

		public float Width;        /**< Width of barcode (points) */
		public float Height;       /**< Height of barcode (points) */

		public bool IsEmpty;       /**< Empty data flag */
		public bool IsDataValid;   /**< Valid data flag */

		public List<DrawingPrimitive> Primitives; /**< List of drawing primitives */
	}
}
