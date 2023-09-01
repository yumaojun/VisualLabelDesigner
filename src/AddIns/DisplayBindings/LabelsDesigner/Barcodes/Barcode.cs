// Copyright (c) 2023 余茂军 <yumaojun@gmail.com> All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Barcodes
{
	/// <summary>
	/// The Barcode class is the base class for all barcode implementations. Barcode类是所有条形码实现的基类
	/// This class provides the public interfaces and basic infrastructure for all barcode implementations. 此类为所有条形码实现提供公共接口和基本基础设施
	/// Implementations would not typically directly implement this class, 实现通常不会直接实现此类，
	/// but instead would implement either Barcode1dBase(for 1D symbologies) or Barcode2dBase(for 2D symbologies). 而是实现Barcode1dBase（用于1D符号体系）或Barcode2dBase（用于2D符号体系）
	/// </summary>
	public abstract class Barcode
	{
		private BarcodeData _data;
		private float _width;
		private float _height;

		public Barcode()
		{
			_data = new BarcodeData(false, false, 0f, 0f, true, false);
		}

		public bool Checksum
		{
			get => _data.ChecksumFlag;
			set => _data.ChecksumFlag = value;
		}

		public bool ShowText
		{
			get => _data.ShowTextFlag;
			set => _data.ShowTextFlag = value;
		}

		public bool IsEmpty
		{
			get => _data.IsEmpty;
			set => _data.IsEmpty = value;
		}

		public bool IsDataValid
		{
			get => _data.IsDataValid;
			set => _data.IsDataValid = value;
		}

		public float Width
		{
			get => _width;
			set => _width = value;
		}

		public float Height
		{
			get => _height;
			set => _height = value;
		}

		/**
		 * Static Code39 barcode creation method
		 *
		 * Used by glbarcode::BarcodeFactory
		 */
		//public abstract Barcode Create();

		/// <summary>
		/// Build barcode from data. 由子类实现
		/// </summary>
		/// <param name="data"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		/// <returns></returns>
		public abstract Barcode Build(string data, float w = 0, float h = 0);

		/// <summary>
		/// Render barcode using given Renderer object. 使用给定的Renderer对象渲染条形码
		/// </summary>
		/// <param name="renderer"></param>
		public void Render(Renderer renderer)
		{
			renderer.Render(_data.Width, _data.Height, _data.Primitives);
		}

		/// <summary>
		/// Clear drawing primitives.
		/// </summary>
		protected void Clear()
		{
			_data.Primitives.Clear();
		}

		/// <summary>
		/// Add line drawing primitive
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		protected void AddLine(float x, float y, float w, float h)
		{
			_data.Primitives.Add(new DrawingPrimitiveLine(x, y, w, h));
		}

		/// <summary>
		/// Add box drawing primitive 添加方框图图元
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		protected void AddBox(float x, float y, float w, float h)
		{
			_data.Primitives.Add(new DrawingPrimitiveBox(x, y, w, h));
		}

		/// <summary>
		/// Add text drawing primitive 添加文本图形图元
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="size"></param>
		/// <param name="text"></param>
		protected void AddText(float x, float y, float size, string text)
		{
			_data.Primitives.Add(new DrawingPrimitiveText(x, y, size, text));
		}

		/// <summary>
		/// Add ring drawing primitive 添加环形图形图元
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="r"></param>
		/// <param name="w"></param>
		protected void AddRing(float x, float y, float r, float w)
		{
			_data.Primitives.Add(new DrawingPrimitiveRing(x, y, r, w));
		}

		/// <summary>
		/// Add hexagon drawing primitive 添加六边形绘图图元
		/// To be used by build() implementations during vectorization. 在矢量化过程中由build()实现使用
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="h"></param>
		protected void AddHexagon(float x, float y, float h)
		{
			_data.Primitives.Add(new DrawingPrimitiveHexagon(x, y, h));
		}
	}
}
