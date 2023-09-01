// Copyright (c) 2023 余茂军 <yumaojun@gmail.com> All rights reserved.

namespace YProgramStudio.LabelsDesigner.Barcodes
{
	public class DrawingPrimitiveHexagon : DrawingPrimitive
	{
		private float _h;

		public float Height { get => _h; set => _h = value; }

		public DrawingPrimitiveHexagon(float x, float y) : base(x, y)
		{
		}

		public DrawingPrimitiveHexagon(float x, float y, float h) : base(x, y)
		{
			_h = h;
		}
	}
}