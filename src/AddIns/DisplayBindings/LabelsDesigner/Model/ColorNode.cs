using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 颜色
	/// </summary>
	public class ColorNode : IEquatable<ColorNode>
	{
		private bool _isField;
		private SKColor _color;
		private String _key;

		public bool IsField
		{
			get => _isField;
			set => _isField = value;
		}

		public SKColor Color
		{
			get => _color;
			set => _color = value;
		}

		public String Key
		{
			get => _key;
			set => _key = value;
		}

		public ColorNode()
		{
			_isField = false;
			_color = System.Drawing.Color.FromArgb(0x00000000).ToSKColor();
			_key = string.Empty;
		}

		public ColorNode(bool isField, SKColor color, string key)
		{
			_isField = isField;
			_color = color;
			_key = key;
		}

		// TODO: *ColorNode(bool isField, uint rgba, string key)需要进一步测试
		public ColorNode(bool isField, uint rgba, string key)
		{
			_isField = isField;
			//var r = (rgba >> 24) & 0xFF;
			//var g = (rgba >> 16) & 0xFF;
			//var b = (rgba >> 8) & 0xFF;
			//var a = (rgba) & 0xFF;
			_color = new SKColor((byte)((rgba >> 24) & 0xFF), (byte)((rgba >> 16) & 0xFF), (byte)((rgba >> 8) & 0xFF), (byte)((rgba) & 0xFF));
			_key = key;
		}

		public ColorNode(SKColor color)
		{
			_isField = false;
			_color = color;
			_key = string.Empty;
		}

		public ColorNode(string key) { }

		/// <summary>
		/// TODO: *uint Rgba()需要经过进一步测试
		/// </summary>
		/// <returns></returns>
		public uint Rgba()
		{
			uint c = (uint)(_color.Red << 24 | _color.Green << 16 | _color.Blue << 8 | _color.Alpha);
			return c;
		}

		// TODO: *GetColor需要继续处理从record和variables取值
		public SKColor GetColor(Backends.Merge.Record record, Variables variables)
		{
			SKColor value = new SKColor(192, 192, 192, 128);

			bool haveRecordField = _isField && record != null && record.ContainsKey(_key) && !string.IsNullOrEmpty(record[_key]);
			bool haveVariable = _isField && variables != null && variables.ContainsKey(_key) && !string.IsNullOrEmpty(variables[_key].Value());

			if (haveRecordField)
			{
				//value = new SKColor(record[_key]);
			}
			else if (haveVariable)
			{
				//value = new SKColor(variables[_key]);//SKColor((*variables)[mKey].value());
			}
			else if (!_isField)
			{
				value = _color;
			}

			return value;
		}

		public bool Equals(ColorNode other) => _isField == other._isField && _color == other._color && _key == other._key;

		public override bool Equals(object obj) => obj != null && Equals(obj as ColorNode);

		public override int GetHashCode() => _isField.GetHashCode() & _color.GetHashCode() & _key.GetHashCode();

		public static bool operator ==(ColorNode a, ColorNode c) => a._isField == c._isField && a._color == c._color && a._key == c._key;

		public static bool operator !=(ColorNode a, ColorNode c) => a._isField != c._isField || a._color != c._color || a._key != c._key;
	}
}
