using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YProgramStudio.LabelsDesigner.Backends.Merge;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// 元素对象基类
	/// </summary>
	public class ModelObject
	{
		protected const float _slopPixels = 2;

		protected static int _nextId;
		protected int _id;
		protected SKMatrix _matrix;

		protected bool _selected;
		protected Distance _x0;
		protected Distance _y0;
		protected Distance _width;
		protected Distance _height;
		protected bool _lockAspectRatio;

		protected bool _shadowState;
		protected Distance _shadowX;
		protected Distance _shadowY;
		protected float _shadowOpacity;
		protected ColorNode _shadowColorNode;

		protected List<Handle> _handles = new List<Handle>();
		protected Outline _outline;

		public event EventHandler Changed;
		public event EventHandler Moved;

		protected virtual void OnChanged(object render, EventArgs e)
		{
			Changed?.Invoke(render, e);
		}

		protected virtual void OnMoved(object render, EventArgs e)
		{
			Moved?.Invoke(render, e);
		}

		protected ModelObject()
		{
			_id = _nextId++;

			_x0 = 0;
			_y0 = 0;
			_width = 0;
			_height = 0;
			_lockAspectRatio = false;
			_matrix = SKMatrix.CreateIdentity();

			_shadowState = false;
			_shadowX = 1.3f;
			_shadowY = 1.3f;
			_shadowColorNode = new ColorNode(System.Drawing.Color.FromArgb(0, 0, 0).ToSKColor());
			_shadowOpacity = 0.5f;

			_selected = false;

			_outline = null;
		}

		protected ModelObject(Distance x0, Distance y0, Distance w, Distance h,
						 bool lockAspectRatio, //  = false

						 SKMatrix matrix //  = null
		)
		{
			_id = _nextId++;

			_x0 = x0;
			_y0 = y0;
			_width = w;
			_height = h;
			_lockAspectRatio = lockAspectRatio;
			_matrix = matrix;

			_shadowState = false;
			_shadowX = 1.3f;
			_shadowY = 1.3f;
			_shadowColorNode = new ColorNode(System.Drawing.Color.FromArgb(0, 0, 0).ToSKColor());
			_shadowOpacity = 0.5f;

			_selected = false;

			_outline = null;
		}

		protected ModelObject(Distance x0, Distance y0, Distance w, Distance h,
						 bool lockAspectRatio, //  = false

						 SKMatrix matrix, //  = null
						 bool shadowState, //  = false

						 Distance shadowX,
						 Distance shadowY,
						 float shadowOpacity = 1.0F,

						 ColorNode shadowColorNode = null)
		{
			_id = _nextId++;

			_x0 = x0;
			_y0 = y0;
			_width = w;
			_height = h;
			_lockAspectRatio = lockAspectRatio;
			_matrix = matrix;

			_shadowState = shadowState;
			_shadowX = shadowX;
			_shadowY = shadowY;
			_shadowColorNode = shadowColorNode;
			_shadowOpacity = shadowOpacity;

			_selected = false;
			_outline = null;
		}

		protected ModelObject(ModelObject modelObject)
		{
		}

		// Object duplication
		public virtual ModelObject Clone()
		{
			return new ModelObject(this);
		}

		public object Parent { get; set; } // TODO: *QObject特有的，暂不清楚在哪使用？

		/// <summary>
		/// 构造函数传入的矩阵
		/// </summary>
		public SKMatrix Matrix
		{
			get => _matrix;
			set
			{
				if (_matrix != value)
				{
					_matrix = value;
					Changed?.Invoke(this, null);
				}
			}
		}

		public bool Selected
		{
			get => _selected;
			set => _selected = value;
		}

		public Distance X0
		{
			get => _x0;
			set
			{
				if (_x0 != value)
				{
					_x0 = value;
					Moved?.Invoke(this, null);
				}
			}
		}

		public Distance Y0
		{
			get => _y0;
			set
			{
				if (_y0 != value)
				{
					_y0 = value;
					Moved?.Invoke(this, null);
				}
			}
		}

		// Overridden by concrete class
		public virtual Distance LineWidth
		{
			get => 0;
			set => Changed?.Invoke(this, null);
		}

		public Distance Width
		{
			get => _width;
			set
			{
				if (_width != value)
				{
					_width = value;
					SizeUpdated();
					Changed?.Invoke(this, new EventArgs());
				}
			}
		}

		public Distance Height
		{
			get => _height;
			set
			{
				if (_height != value)
				{
					_height = value;
					SizeUpdated();
					Changed?.Invoke(this, new EventArgs());
				}
			}
		}

		public bool LockAspectRatio
		{
			get => _lockAspectRatio;
			set
			{
				if (_lockAspectRatio != value)
				{
					_lockAspectRatio = value;
					Changed?.Invoke(this, new EventArgs());
				}
			}
		}

		protected virtual void DrawShadow(SKCanvas painter, bool inEditor, Record record, Variables variables)
		{
			// Nothing
		}

		protected virtual void DrawObject(SKCanvas painter, bool inEditor, Record record, Variables variables)
		{
			// Nothing
		}

		/// <summary>
		/// 判断坐标点是否落在此对象上
		/// </summary>
		protected virtual SKPath HoverPath(float scale)
		{
			return new SKPath(); // Nothing
		}

		// Default sizeUpdated implementation.
		protected virtual void SizeUpdated()
		{
			// Empty
		}

		/// Is one of this object's handles locate at x,y?  If so, return it.
		///
		public Handle HandleAt(float scale, Distance x, Distance y)
		{
			if (_selected)
			{
				SKPoint p = new SKPoint(x.Pt(), y.Pt());
				p -= new SKPoint(_x0.Pt(), _y0.Pt()); // Translate point to x0,y0

				foreach (Handle handle in _handles)
				{
					//SKPath handlePath = _matrix.m(handle.Path(scale)); // TODO: _matrix.map(path)对应是什么？

					SKPath handlePath = handle.Path(scale);


					if (handlePath.Contains(p.X, p.Y))
					{
						return handle;
					}
				}
			}

			return null;
		}

		public void Draw(SKCanvas painter, bool inEditor, Record record, Variables variables)
		{
			painter.Save();
			painter.Translate(_x0.Pt(), _y0.Pt());

			if (_shadowState)
			{
				painter.Save();
				painter.Translate(_shadowX.Pt(), _shadowY.Pt());
				painter.SetMatrix(SKMatrix.Concat(painter.TotalMatrix, _matrix)); // 合并矩阵到painter中
				DrawShadow(painter, inEditor, record, variables);
				painter.Restore();
			}

			painter.SetMatrix(SKMatrix.Concat(painter.TotalMatrix, _matrix)); // 合并矩阵到painter中
			DrawObject(painter, inEditor, record, variables);

			painter.Restore();
		}

		/// <summary>
		/// Draw selection highlights
		/// </summary>
		/// <param name="painter"></param>
		/// <param name="scale"></param>
		public void DrawSelectionHighlight(SKCanvas painter, float scale)
		{
			painter.Save();

			painter.Translate(_x0.Pt(), _y0.Pt());
			painter.SetMatrix(SKMatrix.Concat(painter.TotalMatrix, _matrix)); // 合并矩阵到painter中

			if (_outline != null)
			{
				_outline.Draw(painter);
			}

			foreach (Handle handle in _handles)
			{
				handle.Draw(painter, scale);
			}

			painter.Restore();
		}

		///
		/// Set Size
		///
		public void SetSize(Distance w, Distance h)
		{
			_width = w;
			_height = h;

			SizeUpdated();
			Changed?.Invoke(this, new EventArgs());
		}

		///
		/// Set Size
		///
		public void SetSize(Size size)
		{
			_width = size.Width;
			_height = size.Height;

			SizeUpdated();
			Changed?.Invoke(this, new EventArgs());
		}

		///
		/// Set Size (But Maintain Current Aspect Ratio)
		///
		public void SetSizeHonorAspect(Distance w, Distance h)
		{
			float aspectRatio = _height / _width;
			Distance wNew = w;
			Distance hNew = h;

			if (h > (w * aspectRatio))
			{
				hNew = w * aspectRatio;
			}
			else
			{
				wNew = h / aspectRatio;
			}

			SetSize(wNew, hNew);
		}

		///
		/// Set Width (But Maintain Current Aspect Ratio)
		///
		public void SetWHonorAspect(Distance w)
		{
			float aspectRatio = _height / _width;
			Distance h = w * aspectRatio;

			if ((_width != w) || (_height != h))
			{
				_width = w;
				_height = h;

				SizeUpdated();
				Changed?.Invoke(this, new EventArgs());
			}
		}

		///
		/// Set Height (But Maintain Current Aspect Ratio)
		///
		public void SetHHonorAspect(Distance h)
		{
			float aspectRatio = _height / _width;
			Distance w = h / aspectRatio;

			if ((_width != w) || (_height != h))
			{
				_width = w;
				_height = h;

				SizeUpdated();
				Changed?.Invoke(this, new EventArgs());
			}
		}

		///
		/// Set Absolute Position
		///
		public void SetPosition(Distance x0, Distance y0)
		{
			if ((_x0 != x0) || (_y0 != y0))
			{
				_x0 = x0;
				_y0 = y0;

				Moved?.Invoke(this, new EventArgs());
			}
		}

		///
		/// Set Relative Position
		///
		public void SetPositionRelative(Distance dx, Distance dy)
		{
			if ((dx != 0) || (dy != 0))
			{
				_x0 += dx;
				_y0 += dy;

				Moved?.Invoke(this, new EventArgs());
			}
		}

		///
		/// Get Extent of Bounding Box
		///
		public Region GetExtent()
		{
			SKPoint a1 = new SKPoint((-LineWidth / 2f).Pt(), (-LineWidth / 2f).Pt());
			SKPoint a2 = new SKPoint((Width + LineWidth / 2f).Pt(), (-LineWidth / 2f).Pt());
			SKPoint a3 = new SKPoint((Width + LineWidth / 2f).Pt(), (Height + LineWidth / 2f).Pt());
			SKPoint a4 = new SKPoint((-LineWidth / 2f).Pt(), (Height + LineWidth / 2f).Pt());

			a1 = _matrix.MapPoint(a1);
			a2 = _matrix.MapPoint(a2);
			a3 = _matrix.MapPoint(a3);
			a4 = _matrix.MapPoint(a4);

			Region region = new Region();
			region.X1 = (Math.Min(a1.X, Math.Min(a2.X, Math.Min(a3.X, a4.X))) + _x0);
			region.Y1 = (Math.Min(a1.Y, Math.Min(a2.Y, Math.Min(a3.Y, a4.Y))) + _y0);
			region.X2 = (Math.Max(a1.X, Math.Max(a2.X, Math.Max(a3.X, a4.X))) + _x0);
			region.Y2 = (Math.Max(a1.Y, Math.Max(a2.Y, Math.Max(a3.Y, a4.Y))) + _y0);

			return region;
		}

		/// <summary>
		/// 判断坐标点是否落在当前对象上
		/// </summary>
		public bool IsLocatedAt(float scale, Distance x, Distance y)
		{
			SKPoint p = new SKPoint(x.Pt(), y.Pt());

			/*
			 * Change point to object relative coordinates
			 */
			p -= new SKPoint(_x0.Pt(), _y0.Pt()); // Translate point to x0,y0
			SKMatrix inverseMatrix;
			bool success = _matrix.TryInvert(out inverseMatrix);
			p = inverseMatrix.MapPoint(p);

			if (HoverPath(scale).Contains(p.X, p.Y))
			{
				return true;
			}
			else if (Selected && _outline != null)
			{
				if (_outline.HoverPath(scale).Contains(p.X, p.Y))
				{
					return true;
				}
			}

			return false;
		}
	}
}
