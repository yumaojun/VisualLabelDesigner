using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 自定义可排序点
	/// </summary>
	public class Point : IComparable<Point>
	{
		private Distance _x;
		private Distance _y;

		public Point()
		{
			_x = new Distance(0);
			_y = new Distance(0);
		}

		public Point(Distance x, Distance y)
		{
			_x = x;
			_y = y;
		}

		public Distance X()
		{
			return _x;
		}

		public Distance Y()
		{
			return _y;
		}

		public int CompareTo(Point other)
		{
			if (this > other)
			{
				return 1;
			}
			else if (this < other)
			{
				return -1;
			}
			else
			{
				return 0;
			}
		}

		public static bool operator <(Point a, Point other) => a._y < other._y || (a._y == other._y && a._x < other._x);

		public static bool operator >(Point a, Point other) => other._y < a._y || (a._y == other._y && other._x < a._x);
	}
}
