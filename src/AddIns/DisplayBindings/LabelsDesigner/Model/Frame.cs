using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 框的抽象基类
	/// </summary>
	public abstract class Frame
	{
		private string _id;
		private int _labelsCount;
		private string _layoutDescription;

		private List<Layout> _layouts;
		private List<Markup> _markups;

		/// <summary>
		/// 默认构造函数（Default id=0）
		/// </summary>
		/// <param name="id"></param>
		public Frame(string id = "0")
		{
			_id = id;
			_labelsCount = 0;
			_layoutDescription = string.Empty;
			_layouts = new List<Layout>();
			_markups = new List<Markup>();
		}

		/// <summary>
		/// 复制构造函数
		/// </summary>
		/// <param name="other"></param>
		public Frame(Frame other) : this(other.Id)
		{
			foreach (Layout layout in other._layouts)
			{
				AddLayout(layout);
			}

			foreach (Markup markup in other._markups)
			{
				AddMarkup(markup.Clone());
			}
		}

		/// <summary>
		/// 标签的坐标点集合
		/// </summary>
		/// <returns></returns>
		public virtual Point[] GetOrigins()
		{
			Point[] origins = new Point[LabelsCount];
			int i = 0;
			foreach (Layout layout in _layouts)
			{
				for (int iy = 0; iy < layout.Ny(); iy++)
				{
					for (int ix = 0; ix < layout.Nx(); ix++)
					{
						origins[i++] = new Point(ix * layout.Dx() + layout.X0(), iy * layout.Dy() + layout.Y0());
					}
				}
			}
			Array.Sort(origins); // 从左上到右下排序
			return origins;
		}

		public virtual void AddLayout(Layout layout)
		{
			_layouts.Add(layout);

			// Update total number of labels
			_labelsCount += layout.Nx() * layout.Ny();

			// Update layout description
			if (_layouts.Count == 1)
			{
				//Translators: 1 = number of labels across a page,
				//             2 = number of labels down a page,
				//             3 = total number of labels on a page (sheet).
				_layoutDescription = TranslateHelper.Tr($"{layout.Nx()} x {layout.Ny()} ({LabelsCount} per sheet)");
			}
			else
			{
				/* Translators: 1 is the total number of labels on a page (sheet). */
				_layoutDescription = TranslateHelper.Tr($"{LabelsCount} per sheet");
			}
		}

		public virtual void AddMarkup(Markup markup)
		{
			_markups.Add(markup);
		}

		public virtual List<Layout> Layouts()
		{
			return _layouts;
		}

		public virtual List<Markup> Markups()
		{
			return _markups;
		}

		public string Id { get => _id; }

		public int LabelsCount { get => _labelsCount; }

		public string LayoutDescription { get => _layoutDescription; }

		public abstract Distance Width { get; }

		public abstract Distance Height { get; set; }

		public abstract Frame Clone();

		public abstract bool IsSimilarTo(Frame other);

		public abstract string SizeDescription(Units units);

		public abstract SkiaSharp.SKPath Path();

		public abstract SkiaSharp.SKPath ClipPath();

		public abstract SkiaSharp.SKPath MarginPath(Distance xSize, Distance ySize);
	}
}
