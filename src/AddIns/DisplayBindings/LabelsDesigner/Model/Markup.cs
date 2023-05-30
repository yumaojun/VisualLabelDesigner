using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 标记-Markup
	/// </summary>
	public abstract class Markup
	{
		// Skia路径
		protected SkiaSharp.SKPath _path;

		/// <summary>
		/// 父类的无参构造函数将会被隐式地调用
		/// </summary>
		public Markup()
		{
			_path = new SkiaSharp.SKPath();
		}

		/// <summary>
		/// 复制
		/// </summary>
		/// <returns></returns>
		public abstract Markup Clone();

		/// <summary>
		/// 路径
		/// </summary>
		/// <param name="frame"></param>
		/// <returns></returns>
		public virtual SkiaSharp.SKPath Path(Frame frame)
		{
			return _path;
		}
	}
}
