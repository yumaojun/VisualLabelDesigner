// Copyright (c) 2023 余茂军 <yumaojun@gmail.com> All rights reserved.

namespace YProgramStudio.LabelsDesigner.Barcodes
{
	public class DrawingPrimitiveBox: DrawingPrimitive
	{
		private float _w;
		private float _h;

		public float Width { get => _w; set => _w = value; }

		public float Height { get => _h; set => _h = value; }

		public DrawingPrimitiveBox(float x, float y, float w, float h) : base(x, y)
		{
			_w = w;
			_h = h;
		}
	}
}