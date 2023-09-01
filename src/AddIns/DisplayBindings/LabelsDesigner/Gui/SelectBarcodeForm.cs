using ICSharpCode.SharpDevelop;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YProgramStudio.LabelsDesigner.Backends.Barcode;
using YProgramStudio.LabelsDesigner.Model;

namespace YProgramStudio.LabelsDesigner.Gui
{
	/// <summary>
	/// More barcodes
	/// </summary>
	public partial class SelectBarcodeForm : Form
	{
		private const int physicalDpiX = 158; // TODO: *获取设备物理DPI
		private List<Backends.Barcode.BarcodeStyle> _styleList;
		private Backends.Barcode.BarcodeStyle _bcStyle;
		private ModelBarcodeObject _createObject;

		public List<Backends.Barcode.BarcodeStyle> StyleList { get => _styleList; set => _styleList = value; }
		public BarcodeStyle BcStyle { get => _bcStyle; set => _bcStyle = value; }

		public SelectBarcodeForm()
		{
			InitializeComponent();
			Init();
		}

		private void Init()
		{
			//Barcodes.Factory.Init();
			Backends.Barcode.Backends.Init();
			StyleList = Backends.Barcode.Backends.StyleList;
			BcStyle = StyleList.FirstOrDefault();
			listBox1.DisplayMember = "Name";
			listBox1.ValueMember = "FullId";
			listBox1.DataSource = StyleList;
			CreateCurrentBarcode();
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if ((BcStyle = listBox1.SelectedItem as Backends.Barcode.BarcodeStyle) != null)
			{
				CreateCurrentBarcode(BcStyle);
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if ((BcStyle = listBox1.SelectedItem as Backends.Barcode.BarcodeStyle) != null)
			{
				GetEditor()?.CreateBarcodeMode(BcStyle);
			}

			Close();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void CreateCurrentBarcode(BarcodeStyle bcStyle = null)
		{
			_createObject = new ModelBarcodeObject();
			if (bcStyle != null)
			{
				_createObject.BcStyle = BcStyle;
			}
			_createObject.BcData = _createObject.BcStyle.DefaultDigits;

			var scale = 1 * physicalDpiX / Constants.PTS_PER_INCH;

			var x0 = (pictureBox1.Width - (_createObject.Width.Pt() * scale)) / scale / 2;
			var y0 = (pictureBox1.Height - (_createObject.Height.Pt() * scale)) / scale / 2;
			_createObject.SetPosition(x0, y0);

			using (SKBitmap bitmap = new SKBitmap(pictureBox1.Width, pictureBox1.Height))
			using (SkiaSharp.SKCanvas canvas = new SkiaSharp.SKCanvas(bitmap))
			{
				canvas.Scale(scale);
				_createObject.Draw(canvas, false, null, null);
				pictureBox1.Image = bitmap.ToBitmap();
			}
		}

		/// <summary>
		/// 找到当前文档（null?）
		/// </summary>
		/// <returns></returns>
		protected LabelEditor GetEditor()
		{
			LabelEditor labelEditor = null;
			LabelViewContent viewContent = SD.Workbench.ActiveViewContent as LabelViewContent;
			if (viewContent != null)
			{
				LabelDesignerPanel designerPanel = viewContent.Control as LabelDesignerPanel;
				var tableLayoutPanel = designerPanel.Controls.Find("tableLayoutPanel1", false).First();
				var mainPanel = tableLayoutPanel.Controls.Find("mainPanel", false).First() as Panel;
				labelEditor = mainPanel.Controls.Find("labelEditor", false).First() as LabelEditor;
			}
			return labelEditor;
		}
	}
}
