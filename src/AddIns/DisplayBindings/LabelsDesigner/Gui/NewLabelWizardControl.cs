using ICSharpCode.SharpDevelop.Gui.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YProgramStudio.LabelsDesigner.Model;

namespace YProgramStudio.LabelsDesigner.Gui
{
	public partial class NewLabelWizardControl : AbstractWizardPanel
	{
		public NewLabelWizardControl()
		{
			InitializeComponent();
			Init();
		}

		private void Init()
		{
			//Db.Init();
			List<Category> categories = Db.Categories;
			List<Paper> papers = Db.Papers;
			List<Vendor> vendors = Db.Vendors;
			List<Template> templates = Db.Templates;
			comboBox1.DataSource = categories;
			comboBox1.DisplayMember = "Name";
			comboBox1.ValueMember = "Id";
			listBox1.DataSource = templates;
			listBox1.DisplayMember = "Name";
		}

		protected void radioButton1_Click(object render, EventArgs e)
		{
			listBox1.SelectedIndex = -1;
			listBox1.BackColor = Color.FromArgb(240, 240, 240);
			comboBox1.Enabled = listBox1.Enabled = templateControl1.Visible = label4.Visible = label5.Visible = false;
		}

		protected void radioButton2_Click(object render, EventArgs e)
		{
			listBox1.SelectedIndex = 0;
			listBox1.ResetBackColor();
			comboBox1.Enabled = listBox1.Enabled = templateControl1.Visible = label4.Visible = label5.Visible = true;
		}

		protected void comboBox1_SelectedIndexChanged(object render, EventArgs e)
		{
			var category = comboBox1.SelectedItem as Category;
			var selectTemplates = Db.Templates.Where(x => x.CategoryIds.Contains(category.Id)).ToList();
			listBox1.DataSource = selectTemplates;
		}

		protected void listBox1_SelectedIndexChanged(object render, EventArgs e)
		{
			var template = listBox1.SelectedItem as Template;
			templateControl1.UpdateView(template);
		}
	}
}
