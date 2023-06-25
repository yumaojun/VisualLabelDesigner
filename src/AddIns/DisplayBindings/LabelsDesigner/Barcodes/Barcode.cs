using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Barcodes
{
	/// <summary>
	/// The Barcode class is the base class for all barcode implementations.  This class
	/// provides the public interfaces and basic infrastructure for all barcode implementations.
	/// Implementations would not typically directly implement this class, but instead would implement
	/// either Barcode1dBase(for 1D symbologies) or Barcode2dBase(for 2D symbologies).
	/// </summary>
	public class Barcode
	{
		private BarcodeData data;
		private float _width;
		private float _height;

		public Barcode()
		{
			data = new BarcodeData(false, false, 0f, 0f, true, false);
		}

		//public Barcode(Barcodes.Renderer renderer) { }

		public bool Checksum { get => data.ChecksumFlag; set => data.ChecksumFlag = value; }

		public bool ShowText { get => data.ShowTextFlag; set => data.ShowTextFlag = value; }

		public bool IsEmpty { get => data.IsEmpty; set => data.IsEmpty = value; }

		public bool IsDataValid { get => data.IsDataValid; set => data.IsDataValid = value; }

		public float Width { get => _width; set => _width = value; }

		public float Height { get => _height; set => _height = value; }

		// 由子类实现
		public virtual Barcode Build(string data, float w = 0, float h = 0)
		{
			throw new NotImplementedException();
		}

		// Render barcode using given Renderer object.
		public void Render(Renderer renderer)
		{
			renderer.Render(data.Width, data.Height, data.Primitives);
		}

		protected void Clear()
		{
			data.Primitives.Clear();
		}

		protected void AddLine(float x, float y, float w, float h)
		{
			data.Primitives.Add(new DrawingPrimitiveLine(x, y, w, h));
		}

		protected void AddBox(float x, float y, float w, float h)
		{
			data.Primitives.Add(new DrawingPrimitiveBox(x, y, w, h));
		}

		protected void AddText(float x, float y, float size, string text)
		{
			data.Primitives.Add(new DrawingPrimitiveText(x, y, size, text));
		}

		protected void AddRing(float x, float y, float r, float w)
		{
			data.Primitives.Add(new DrawingPrimitiveRing(x, y, r, w));
		}

		protected void AddHexagon(float x, float y, float h)
		{
			data.Primitives.Add(new DrawingPrimitiveHexagon(x, y, h));
		}
	}
}
