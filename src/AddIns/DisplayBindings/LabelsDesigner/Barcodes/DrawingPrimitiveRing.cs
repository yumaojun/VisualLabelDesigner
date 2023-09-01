// Copyright (c) 2023 余茂军 <yumaojun@gmail.com> All rights reserved.

namespace YProgramStudio.LabelsDesigner.Barcodes
{
	public class DrawingPrimitiveRing : DrawingPrimitive
	{
		private float _w;
		private float _r;

		public float Width { get => _w; set => _w = value; }

		public float Radius { get => _r; set => _r = value; }

		public DrawingPrimitiveRing(float x, float y) : base(x, y)
		{
		}

		public DrawingPrimitiveRing(float x, float y, float r, float w) : base(x, y)
		{
			_w = w;
			_r = r;
		}
	}
}