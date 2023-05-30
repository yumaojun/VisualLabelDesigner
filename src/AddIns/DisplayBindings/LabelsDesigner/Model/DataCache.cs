using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	public class DataCache
	{
		private Dictionary<string, SKImage> _imageMap = new Dictionary<string, SKImage>();
		private Dictionary<string, byte[]> _svgMap = new Dictionary<string, byte[]>();

		public bool HasImage(string name)
		{
			return _imageMap.ContainsKey(name);
		}

		public SKImage GetImage(string name)
		{
			return _imageMap[name];
		}

		public void AddImage(string name, SKImage image)
		{
			_imageMap[name] = image;
		}

		public List<string> ImageNames()
		{
			return _imageMap.Keys.ToList();
		}

		public bool HasSvg(string name)
		{
			return _svgMap.ContainsKey(name);
		}

		public byte[] GetSvg(string name)
		{
			return _svgMap[name];
		}

		public void AddSvg(string name, byte[] svg)
		{
			_svgMap[name] = svg;
		}

		public List<string> SvgNames()
		{
			return _svgMap.Keys.ToList();
		}
	}
}
