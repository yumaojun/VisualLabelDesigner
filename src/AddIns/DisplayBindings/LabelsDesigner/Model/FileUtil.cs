using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 文件帮助类
	/// </summary>
	public class FileUtil
	{
		/// <summary>
		/// 返回系统模板文件的存放根路径
		/// </summary>
		/// <returns></returns>
		public static string SystemTemplatesDir()
		{
			return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "templates");
		}

		public static string UserTemplatesDir()
		{
			return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "templates");// TODO: *FileUtil.UserTemplatesDir()更改为用户目录
		}

		public static string CurrentExecutingDir()
		{
			var dllPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

			var v1 = Path.GetFullPath(dllPath);
			var v2 = Path.GetDirectoryName(dllPath);
			return v2;
		}
	}
}
