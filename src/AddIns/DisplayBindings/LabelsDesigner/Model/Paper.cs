using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 页
	/// </summary>
	public class Paper
	{
		public Paper(string id, string name, Distance width, Distance height, string pwgSize)
		{
			Id = id;
			Name = name;
			Width = width;
			Height = height;
			PwgSize = pwgSize;
		}

		public string Id { get; private set; }
		public string Name { get; private set; }
		public Distance Width { get; private set; }
		public Distance Height { get; private set; }
		public string PwgSize { get; private set; }

		public bool IsSizeIso()
		{
			return PwgSize.StartsWith("iso_");
		}

		public bool IsSizeUs()
		{
			return PwgSize.StartsWith("na_");
		}
	}
}
