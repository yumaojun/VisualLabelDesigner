using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YProgramStudio.LabelsDesigner.Backends.Merge;

namespace YProgramStudio.LabelsDesigner.Model
{
	/// <summary>
	/// Label模型
	/// </summary>
	public class Model
	{
		#region Private Data

		private bool _modified;
		private string _fileName;
		private Template _template = new Template();
		private bool _rotate;
		private List<ModelObject> _objectList = new List<ModelObject>();

		#endregion

		#region Events

		public event EventHandler Changed;
		public event EventHandler NameChanged;
		public event EventHandler SizeChanged;
		public event EventHandler SelectionChanged;
		public event EventHandler ModifiedChanged;
		public event EventHandler VariablesChanged;
		public event EventHandler MergeChanged;
		public event EventHandler MergeSourceChanged;
		public event EventHandler MergeSelectionChanged;

		#endregion

		#region Event Handler

		private void OnObjectChanged(object render, EventArgs e)
		{
			Modified = true;
			Changed?.Invoke(this, null);
		}

		private void OnObjectMoved(object render, EventArgs e)
		{
			Modified = true;
			Changed?.Invoke(this, null);
		}

		private void OnVariablesChanged()
		{
			Modified = true;
			Changed?.Invoke(this, null);
			VariablesChanged?.Invoke(this, null);
		}

		private void OnMergeSourceChanged()
		{
			Modified = true;
			Changed?.Invoke(this, null);
			MergeSourceChanged?.Invoke(this, null);
		}

		private void OnMergeSelectionChanged()
		{
			Changed?.Invoke(this, null);
			MergeSelectionChanged?.Invoke(this, null);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Width Getter
		/// </summary>
		public Distance Width
		{
			get
			{
				var frame = _template.Frames.FirstOrDefault();
				if (frame != null)
				{
					return _rotate ? frame.Height : frame.Width;
				}
				else
				{
					return Distance.Pt(0F);
				}
			}
		}

		/// <summary>
		/// Height Getter & Setter
		/// </summary>
		public Distance Height
		{
			get
			{
				var frame = _template.Frames.FirstOrDefault();
				if (frame != null)
				{
					return _rotate ? frame.Width : frame.Height;
				}
				else
				{
					return Distance.Pt(0F);
				}
			}
			set
			{
				var frame = _template.Frames.FirstOrDefault();
				if (frame != null)
				{
					frame.Height = value;
					Modified = true;
					Changed?.Invoke(this, null);
					SizeChanged?.Invoke(this, null);
				}
			}
		}

		/// <summary>
		/// Modified status
		/// </summary>
		public bool Modified
		{
			get => _modified;
			set
			{
				_modified = value;
				ModifiedChanged?.Invoke(this, null);
			}
		}

		public string FileName
		{
			get => _fileName;
			set
			{
				if (_fileName != value)
				{
					_fileName = value;
					NameChanged?.Invoke(this, null);
				}
			}
		}

		public Template Template
		{
			get => _template;
			set
			{
				_template = value;
				Modified = true;
				Changed?.Invoke(this, new EventArgs());
				SizeChanged?.Invoke(this, new EventArgs());
				//Settings.addToRecentTemplateList(tmplate->name());
			}
		}

		public bool Rotate
		{
			get => _rotate;
			set
			{
				if (_rotate != value)
				{
					_rotate = value;
					Modified = true;
					Changed?.Invoke(this, null);
					SizeChanged?.Invoke(this, null);
				}
			}
		}

		public Frame Frame
		{
			get => _template.Frames.FirstOrDefault();
		}

		public List<ModelObject> ObjectList
		{
			get => _objectList;
		}

		#endregion

		#region Manage objects

		public void AddObject(ModelObject modelObject)
		{
			//modelObject.SetParent(this);
			modelObject.Parent = this;
			ObjectList.Add(modelObject);

			//connect(object, SIGNAL(changed()), this, SLOT(onObjectChanged()));
			//connect(object, SIGNAL(moved()), this, SLOT(onObjectMoved()));
			modelObject.Changed += OnObjectChanged;
			modelObject.Moved += OnObjectMoved;

			Modified = true;
			Changed?.Invoke(this, null);
		}

		public void DeleteObject(ModelObject modelObject)
		{
			modelObject.Selected = false;
			ObjectList.Remove(modelObject);

			//disconnect(object, nullptr, this, nullptr);

			Modified = true;
			Changed?.Invoke(this, null);

			//delete object;
			modelObject.Changed -= OnObjectChanged;
			modelObject.Moved -= OnObjectMoved;
		}

		public ModelObject ObjectAt(float scale, Distance x, Distance y)
		{
			/* Search object list in reverse order.  I.e. from top to bottom. */
			//var it = ObjectList.Reverse().GetEnumerator();
			//while (it.MoveNext())
			//{
			//	if (it.Current.IsLocatedAt(scale, x, y))
			//	{
			//		return it.Current;
			//	}
			//}
			for (int i = ObjectList.Count - 1; i >= 0; i--)
			{
				if (ObjectList[i].IsLocatedAt(scale, x, y))
				{
					return ObjectList[i];
				}
			}

			return null;
		}

		/// Handle at x,y
		public Handle HandleAt(float scale, Distance x, Distance y)
		{
			foreach (ModelObject modelObject in ObjectList)
			{
				Handle handle = modelObject.HandleAt(scale, x, y);
				if (handle != null)
				{
					return handle;
				}
			}

			return null;
		}

		/// <summary>
		/// Select Object
		/// </summary>
		public void SelectObject(ModelObject modelObject)
		{
			modelObject.Selected = true;
			SelectionChanged?.Invoke(this, null);
		}

		/// <summary>
		/// Unselect Object
		/// </summary>
		public void UnselectObject(ModelObject modelObject)
		{
			modelObject.Selected = false;
			SelectionChanged?.Invoke(this, null);
		}

		/// <summary>
		/// Select All Objects
		/// </summary>
		public void SelectAll()
		{
			foreach (ModelObject modelObject in _objectList)
			{
				modelObject.Selected = true;
			}

			SelectionChanged?.Invoke(this, null);
		}

		///
		/// Unselect All Objects
		///
		public void UnselectAll()
		{
			foreach (ModelObject modelObject in _objectList)
			{
				modelObject.Selected = false;
			}

			SelectionChanged?.Invoke(this, null);
		}

		///
		/// Select Region
		///
		public void SelectRegion(Region region)
		{
			Distance rX1 = Math.Min(region.X1, region.X2);
			Distance rY1 = Math.Min(region.Y1, region.Y2);
			Distance rX2 = Math.Max(region.X1, region.X2);
			Distance rY2 = Math.Max(region.Y1, region.Y2);

			foreach (ModelObject modelObject in _objectList)
			{
				Region objectExtent = modelObject.GetExtent();

				if ((objectExtent.X1 >= rX1) &&
					 (objectExtent.X2 <= rX2) &&
					 (objectExtent.Y1 >= rY1) &&
					 (objectExtent.Y2 <= rY2))
				{
					modelObject.Selected = true;
				}
			}

			SelectionChanged?.Invoke(this, null);
		}

		#endregion

		/// <summary>
		/// Drawing operations
		/// </summary>
		public void Draw(SKCanvas painter, bool inEditor, Record record, Variables variables)
		{
			foreach (ModelObject modelObject in _objectList)
			{
				modelObject.Draw(painter, inEditor, record, variables);
			}
		}

		// TODO: *路径问题需要后续处理
		public string Dir()
		{
			if (string.IsNullOrEmpty(_fileName))
			{
				return AppContext.BaseDirectory;
			}
			else
			{
				var fileInfo = new System.IO.FileInfo(_fileName);
				return fileInfo.FullName;
			}
		}

		public void MoveSelection(Distance dx, Distance dy)
		{
			List<ModelObject> selectedList = GetSelection();

			foreach (ModelObject modelObject in selectedList)
			{
				modelObject.SetPositionRelative(dx, dy);
			}

			Modified = true;
			Changed?.Invoke(this, null);
		}

		public void DeleteSelection()
		{
			List<ModelObject> selectedList = GetSelection();

			foreach (ModelObject modelObject in selectedList)
			{
				DeleteObject(modelObject);
			}

			Modified = true;
			Changed?.Invoke(this, null);
			SelectionChanged?.Invoke(this, null);
		}

		///
		/// Get List of Selected Objects
		///
		public List<ModelObject> GetSelection()
		{
			return _objectList.Where(x => x.Selected).ToList();
		}

		///
		/// Is Selection Atomic?
		///
		public bool IsSelectionAtomic()
		{
			int nSelected = 0;

			foreach (ModelObject modelObject in _objectList)
			{
				if (modelObject.Selected)
				{
					nSelected++;
					if (nSelected > 1)
					{
						return false;
					}
				}
			}

			return nSelected == 1;
		}
	}
}
