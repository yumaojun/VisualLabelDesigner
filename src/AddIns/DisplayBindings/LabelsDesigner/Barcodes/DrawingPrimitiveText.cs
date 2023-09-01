// Copyright (c) 2023 余茂军 <yumaojun@gmail.com> All rights reserved.

namespace YProgramStudio.LabelsDesigner.Barcodes
{
	internal class DrawingPrimitiveText : DrawingPrimitive
	{
		private float _size;
		private string _text;

		public float Size
		{
			get => _size;
			set => _size = value;
		}

		public string Text
		{
			get => _text;
			set => _text = value;
		}

		public DrawingPrimitiveText(float x, float y, float size, string text) : base(x, y)
		{
			_size = size;
			_text = text;
		}
	}
}