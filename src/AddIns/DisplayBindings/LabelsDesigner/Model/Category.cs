using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 分类
	/// </summary>
	public class Category
	{
		public Category(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string Id { get; private set; }
		public string Name { get; private set; }
	}
}
