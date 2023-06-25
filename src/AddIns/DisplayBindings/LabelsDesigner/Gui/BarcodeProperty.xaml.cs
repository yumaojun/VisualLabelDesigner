using ICSharpCode.SharpDevelop;
using ICSharpCode.SharpDevelop.Workbench;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace YProgramStudio.LabelsDesigner.Gui
{
	/// <summary>
	/// 对象属性面板
	/// </summary>
	public partial class BarcodeProperty : UserControl, IPadContent, INotifyPropertyChanged
	{
		private static BarcodeProperty _instance;
		private Model.Model _model;
		private Model.UndoRedoModel _undoRedoModel;
		private Model.ModelObject _object;

		private bool _isShowTextEnabled = true;
		private bool _isChecksumEnabled = true;
		private bool _isShowText = true;
		private bool _isChecksum = true;
		private float _lineWidth = 1.0f;
		private Color _lineColor = Colors.Black;
		private Color _fillColor = Colors.LightGreen;
		private Color _bcColor = Colors.Black;
		private string _text = string.Empty;
		private string _brand = string.Empty;
		private string _part = string.Empty;
		private string _description = string.Empty;
		private float _x0 = 1.00f;
		private float _y0 = 1.00f;
		private float _width = 1.00f;
		private float _height = 1.00f;

		public object Control => this;

		public object InitiallyFocusedControl => null;

		public void Dispose()
		{
		}

		public object GetService(Type serviceType)
		{
			return null;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public static BarcodeProperty Instance => _instance;

		public BarcodeProperty()
		{
			InitializeComponent();
			Loaded += BarcodeProperty_Loaded;
			_instance = this;
		}

		private void BarcodeProperty_Loaded(object sender, RoutedEventArgs e)
		{
			HideAllTabPages();
			labelTabItem.Visibility = Visibility.Visible;
		}

		#region Properties

		public bool IsShowTextEnabled
		{
			get => _isShowTextEnabled;
			set
			{
				_isShowTextEnabled = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsShowTextEnabled"));
			}
		}

		public bool IsChecksumEnabled
		{
			get => _isChecksumEnabled;
			set
			{
				_isChecksumEnabled = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsChecksumEnabled"));
			}
		}

		public bool IsShowTextValue
		{
			get => _isShowText;
			set
			{
				_isShowText = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsShowTextValue"));
			}
		}

		public bool IsChecksumValue
		{
			get => _isChecksum;
			set
			{
				_isChecksum = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsChecksumValue"));
			}
		}

		/// <summary>
		/// 线宽
		/// </summary>
		public float LineWidthValue
		{
			get { return _lineWidth; }
			set
			{
				_lineWidth = value;
				if (_object != null)
				{
					_object.LineWidth = value;
				}
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LineWidthValue"));
			}
		}

		/// <summary>
		/// 线颜色
		/// </summary>
		public Color LineColorValue
		{
			get { return _lineColor; }
			set
			{
				_lineColor = value;
				if (_object is Model.ModelShapeObject shape)
				{
					shape.LineColorNode = new Model.ColorNode(WMColorToSKColor(value));
				}
				else if (_object is Model.ModelLineObject line)
				{
					line.LineColorNode = new Model.ColorNode(WMColorToSKColor(value));
				}
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LineColorValue"));
			}
		}

		/// <summary>
		/// 填充色
		/// </summary>
		public Color FillColorValue
		{
			get { return _fillColor; }
			set
			{
				_fillColor = value;
				if (_object is Model.ModelShapeObject shape)
				{
					shape.FillColorNode = new Model.ColorNode(WMColorToSKColor(value));
				}
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FillColorValue"));
			}
		}

		/// <summary>
		/// 条码颜色
		/// </summary>
		public Color BcColorValue
		{
			get { return _bcColor; }
			set
			{
				_bcColor = value;
				if (_object is Model.ModelBarcodeObject barcode)
				{
					barcode.BcColorNode = new Model.ColorNode(WMColorToSKColor(value));
				}
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BcColorValue"));
			}
		}

		/// <summary>
		/// 文本
		/// </summary>
		public string TextValue
		{
			get { return _text; }
			set
			{
				_text = value;
				if (_object is Model.ModelBarcodeObject barcode)
				{
					barcode.BcData = value;
				}
				else if (_object is Model.ModelTextObject text)
				{
					text.Text = value;
				}
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TextValue"));
			}
		}

		/// <summary>
		/// 品牌
		/// </summary>
		public string Brand
		{
			get { return _brand; }
			set
			{
				_brand = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Brand"));
			}
		}

		/// <summary>
		/// 规格
		/// </summary>
		public string Part
		{
			get { return _part; }
			set
			{
				_part = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Part"));
			}
		}

		/// <summary>
		/// 说明
		/// </summary>
		public string Description
		{
			get { return _description; }
			set
			{
				_description = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Description"));
			}
		}

		/// <summary>
		/// X坐标
		/// </summary>
		public float X0Value
		{
			get => _x0;
			set
			{
				_x0 = value;
				if (_object != null)
				{
					_object.X0 = value; // TODO: _object.X0 = value这里是跟单位有关系的，需要改进
				}
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("X0Value"));
			}
		}

		/// <summary>
		/// Y坐标
		/// </summary>
		public float Y0Value
		{
			get => _y0;
			set
			{
				_y0 = value;
				if (_object != null)
				{
					_object.Y0 = value;
				}
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Y0Value"));
			}
		}

		/// <summary>
		/// 宽度
		/// </summary>
		public float WidthValue
		{
			get => _width;
			set
			{
				_width = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("WidthValue"));
			}
		}

		/// <summary>
		/// 高度
		/// </summary>
		public float HeightValue
		{
			get => _height;
			set
			{
				_height = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HeightValue"));
			}
		}

		#endregion

		// TODO：考虑当viewContent关闭，需要移除事件的订阅；当新增或切换viewContent需要绑定到属性面板
		public void SetModel(Model.Model model, Model.UndoRedoModel undoRedoModel)
		{
			_model = model;
			_undoRedoModel = undoRedoModel;

			if (_model != null)
			{
				_model.SelectionChanged += OnModelSelectionChanged;
				_model.Changed += OnModelChanged;
				Brand = _model.Template.Brand;
				Part = _model.Template.Part;
				Description = _model.Template.Description;
			}
		}

		// 隐藏所有tab页
		private void HideAllTabPages()
		{
			labelTabItem.Visibility = Visibility.Collapsed;
			textTabItem.Visibility = Visibility.Collapsed;
			barcodeTabItem.Visibility = Visibility.Collapsed;
			imageTabItem.Visibility = Visibility.Collapsed;
			lineFillTabItem.Visibility = Visibility.Collapsed;
			posSizeTabItem.Visibility = Visibility.Collapsed;
			shadowTabItem.Visibility = Visibility.Collapsed;
		}

		private void ShowShapeTabPages()
		{
			lineFillTabItem.Visibility = Visibility.Visible;
			posSizeTabItem.Visibility = Visibility.Visible;
			shadowTabItem.Visibility = Visibility.Visible;
			mainTab.SelectedItem = lineFillTabItem;
		}

		// Skia Color 转为 WPF Color
		private Color SKColorToWMColor(SkiaSharp.SKColor skColor)
		{
			return Color.FromArgb(skColor.Alpha, skColor.Red, skColor.Green, skColor.Blue);
		}

		// WPF Color 转为 Skia Color
		private SkiaSharp.SKColor WMColorToSKColor(Color wmColor)
		{
			return System.Drawing.Color.FromArgb(wmColor.A, wmColor.R, wmColor.G, wmColor.B).ToSKColor();
		}

		//private void OnModelSizeChanged(object sender, EventArgs e)
		//{
		//	if (_model.IsSelectionAtomic() && (_object = _model.GetFirstSelectedObject()) != null)
		//	{
		//		if (_object is Model.ModelObject mobject)
		//		{
		//			if (!(_object is Model.ModelLineObject))
		//			{
		//				WidthValue = mobject.Width.Pt();
		//				HeightValue = mobject.Height.Pt();
		//			}
		//		}
		//	}
		//}

		private void OnModelChanged(object sender, EventArgs e)
		{
			if (_model.IsSelectionAtomic() && (_object = _model.GetFirstSelectedObject()) != null)
			{
				if (_object is Model.ModelObject mobject)
				{
					X0Value = mobject.X0.Pt();
					Y0Value = mobject.Y0.Pt();

				}
			}
		}

		// 最终此函数开放给ViewContent使用，以解决当ViewContent生命周期结束时取消事件订阅
		private void OnModelSelectionChanged(object sender, EventArgs e)
		{
			HideAllTabPages();

			if (_model.IsSelectionAtomic() && (_object = _model.GetFirstSelectedObject()) != null)
			{
				if (_object is Model.ModelObject mobject)
				{
					X0Value = mobject.X0.Pt();
					Y0Value = mobject.Y0.Pt();

					//if (!(_object is Model.ModelLineObject))
					//{
					//	WidthValue = mobject.Width.Pt();
					//	HeightValue = mobject.Height.Pt();
					//}
				}

				if (_object is Model.ModelShapeObject shape) // Contains box and ellipse
				{
					ShowShapeTabPages();
					fillGroup.Visibility = Visibility.Visible;
					LineWidthValue = shape.LineWidth.Pt();
					LineColorValue = SKColorToWMColor(shape.LineColorNode.Color);
					FillColorValue = SKColorToWMColor(shape.FillColorNode.Color);
				}
				else if (_object is Model.ModelLineObject line)
				{
					ShowShapeTabPages();
					fillGroup.Visibility = Visibility.Collapsed;
					LineWidthValue = line.LineWidth.Pt();
					LineColorValue = SKColorToWMColor(line.LineColorNode.Color);
					// 长度、角度
				}
				else if (_object is Model.ModelImageObject image)
				{
					imageTabItem.Visibility = Visibility.Visible;
					posSizeTabItem.Visibility = Visibility.Visible;
					shadowTabItem.Visibility = Visibility.Visible;
					mainTab.SelectedItem = imageTabItem;
				}
				else if (_object is Model.ModelTextObject text)
				{
					textTabItem.Visibility = Visibility.Visible;
					posSizeTabItem.Visibility = Visibility.Visible;
					shadowTabItem.Visibility = Visibility.Visible;
					TextValue = text.Text;
					mainTab.SelectedItem = textTabItem;
				}
				else if (_object is Model.ModelBarcodeObject barcode)
				{
					barcodeTabItem.Visibility = Visibility.Visible;
					posSizeTabItem.Visibility = Visibility.Visible;
					IsShowTextValue = barcode.BcTextFlag;
					IsChecksumValue = barcode.BcChecksumFlag;
					BcColorValue = SKColorToWMColor(barcode.BcColorNode.Color);
					TextValue = barcode.BcData;
					mainTab.SelectedItem = barcodeTabItem;
				}
			}
			else
			{
				// Current label's properties
				labelTabItem.Visibility = Visibility.Visible;
				mainTab.SelectedItem = labelTabItem;
			}
		}

		protected void ImageFileButtonClick(object sender, RoutedEventArgs e)
		{
			string path = SD.FileService.BrowseForFolder(
				"请选择一个图片",
				imageFileTextBox.Text);

			if (path != null)
				imageFileTextBox.Text = path;
		}
	}
}
