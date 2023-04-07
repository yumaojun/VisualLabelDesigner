using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualLabelDesigner.ZplTextEditor.Gui
{
	public partial class LabelSizeOptionForm : Form
	{
		public List<LabelFormat> _listLabel { get; set; }
		public LabelFormat _labelFormat { get; set; }

		public LabelSizeOptionForm(List<LabelFormat> labelFormats)
		{
			InitializeComponent();
			this._listLabel = labelFormats;
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.tbName.Text) || string.IsNullOrWhiteSpace(this.tbHeight.Text) || string.IsNullOrWhiteSpace(this.tbWidth.Text))
			{
				MessageBox.Show("Unable to save label format, no name or wrong values!");
				return;
			}
			Unit unit = this.cbUnit.SelectedItem as Unit;
			Measure width = new Measure
			{
				Unit = unit,
				Value = Convert.ToDouble(this.tbWidth.Text.Replace('.', ','))
			};
			Measure height = new Measure
			{
				Unit = unit,
				Value = Convert.ToDouble(this.tbHeight.Text.Replace('.', ','))
			};
			LabelFormat item = new LabelFormat
			{
				Name = this.tbName.Text,
				Width = width,
				Height = height
			};
			this._listLabel.Add(item);
		}

		private void LabelSizeOptionForm_Load(object sender, EventArgs e)
		{
			this.lbLabelFormat.DataSource = this._listLabel;
			this.lbLabelFormat.DisplayMember = "DisplayValue";
			this.lbLabelFormat.SelectedIndexChanged += this.lbLabelFormat_SelectedIndexChanged;
			this.lbLabelFormat.SelectedIndex = -1;
			this.cbUnit.DataSource = Unit.GetAvailableUnit();
			this.cbUnit.DisplayMember = "Name";
			this.cbUnit.ValueMember = "Name";
		}

		private void lbLabelFormat_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.lbLabelFormat.SelectedIndex != -1 && (this.lbLabelFormat.SelectedItem as LabelFormat).Name != "None")
			{
				this.tbName.Text = (this.lbLabelFormat.SelectedItem as LabelFormat).Name;
				this.tbWidth.Text = (this.lbLabelFormat.SelectedItem as LabelFormat).Width.Value.ToString();
				this.tbHeight.Text = (this.lbLabelFormat.SelectedItem as LabelFormat).Height.Value.ToString();
				this.cbUnit.SelectedValue = (this.lbLabelFormat.SelectedItem as LabelFormat).Height.Unit.Name;
			}
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			this.lbLabelFormat.SelectedIndex = -1;
			this.tbName.Clear();
			this.tbWidth.Clear();
			this.tbHeight.Clear();
			this.cbUnit.SelectedIndex = 0;
			this.tbName.Focus();
		}

		private void btnRemove_Click(object sender, EventArgs e)
		{
			btnAdd_Click(null, null);
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			btnSave_Click(null, null);
			btnCancel_Click(null, null);
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
