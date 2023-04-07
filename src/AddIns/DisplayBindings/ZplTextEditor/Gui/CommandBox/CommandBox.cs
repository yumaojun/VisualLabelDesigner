using ICSharpCode.SharpDevelop.Workbench;
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
	public partial class CommandBox : UserControl, IPadContent
	{
		static CommandBox instance;

		public event System.EventHandler ZplCommandMouseHover;

		public event System.EventHandler ZplCommandClick;

		public CommandBox()
		{
			instance = this; // TODO: instance need to dispose() ?
			InitializeComponent();
			if (CommandHelper.Instance!= null){
				CommandHelper.Instance.SubscribeCommandClickEvent(this);
			}
		}

		public object Control => this;

		public object InitiallyFocusedControl => null;

		object IServiceProvider.GetService(Type serviceType)
		{
			return null;
		}

		public static CommandBox Instance
		{
			get
			{
					return instance;
			}
		}

		private void ZplCommand_Click(object sender, EventArgs e)
		{
			this.OnZplCommandClick(sender, e);
		}

		private void ZplCommand_MouseHover(object sender, EventArgs e)
		{
			this.OnZplCommandMouseHover(sender, e);
		}

		protected virtual void OnZplCommandClick(object sender, EventArgs e)
		{
			EventHandler zplCommandClick = this.ZplCommandClick;
			if (zplCommandClick == null)
			{
				return;
			}
			zplCommandClick(sender, e);
		}

		protected virtual void OnZplCommandMouseHover(object sender, EventArgs e)
		{
			EventHandler zplCommandMouseHover = this.ZplCommandMouseHover;
			if (zplCommandMouseHover == null)
			{
				return;
			}
			zplCommandMouseHover(sender, e);
		}

		private void zToolbox_Load(object sender, EventArgs e)
		{
			this.li_ZplCode = Services.ZPLCommand.DeserializeFromXML();
			//this.gbZpl.SortClick += this.GbZpl_SortClick;
			//this.gbZpl.CloseClick += this.OnCloseClick;
			this.TstbSearch_LostFocus(sender, e);
			this.GenerateMenu(null, EventArgs.Empty);
		}

		private void GbZpl_SortClick(object sender, EventArgs e)
		{
			this.tbSearch.Clear();
			this.TstbSearch_LostFocus(sender, e);
			//Settings.Default.ToolboxDisplayOrder = ((Settings.Default.ToolboxDisplayOrder == 0) ? 1 : 0);
			//Settings.Default.Save();
			this.GenerateMenu(sender, e);
		}

		private void GenerateMenu(object sender, EventArgs e)
		{
			base.SuspendLayout();

			List<Services.ZPLCommand> list = new List<Services.ZPLCommand>();
			for (int i = this.tsTool.Items.Count - 1; i >= 0; i += -1)
			{
				this.tsTool.Items.RemoveAt(i);
			}
			//if (Settings.Default.ToolboxDisplayOrder == 1)
			//{
			list = this.li_ZplCode.OrderBy(x => x.Name.Substring(1, 1)).ToList();
			//}
			//else
			//{
			//	list = this.li_ZplCode;
			//}
			string b = string.Empty;
			foreach (Services.ZPLCommand zplcommand in list)
			{
				if (/*Settings.Default.ToolboxDisplayOrder == 0 && */zplcommand.Category != b)
				{
					ToolStripLabel value = new ToolStripLabel(zplcommand.Category)
					{
						Font = new Font("Consolas", 10f, FontStyle.Bold),
						TextAlign = ContentAlignment.MiddleLeft,
						Padding = new Padding(0, 10, 0, 0)
					};
					b = zplcommand.Category;
					this.tsTool.Items.Add(value);
				}
				ToolStripButton toolStripButton = new ToolStripButton(string.Format("{0} - {1}", zplcommand.Name, zplcommand.ShortDescription));
				toolStripButton.TextAlign = ContentAlignment.MiddleLeft;
				toolStripButton.Tag = zplcommand;
				toolStripButton.Click += this.ZplCommand_Click;
				toolStripButton.MouseHover += this.ZplCommand_MouseHover;
				this.tsTool.Items.Add(toolStripButton);
			}

			base.ResumeLayout();
		}

		private void TstbSearch_TextChanged(object sender, EventArgs e)
		{
			base.SuspendLayout();

			foreach (object obj in this.tsTool.Items)
			{
				ToolStripButton toolStripButton = obj as ToolStripButton;
				if (toolStripButton != null)
				{
					if (this.tbSearch.Text != "Search ..." && !string.IsNullOrEmpty(this.tbSearch.Text))
					{
						toolStripButton.Visible = (new System.Text.RegularExpressions.Regex(string.Format("({0})", this.tbSearch.Text), System.Text.RegularExpressions.RegexOptions.IgnoreCase).Match(toolStripButton.Text).Success);
					}
					else
					{
						toolStripButton.Visible = true;
					}
				}
			}

			base.ResumeLayout();
		}

		private void TstbSearch_LostFocus(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.tbSearch.Text))
			{
				this.tbSearch.Text = "Search ...";
				this.tbSearch.ForeColor = Color.Gray;
			}
		}

		private void TstbSearch_GotFocus(object sender, EventArgs e)
		{
			if (this.tbSearch.Text == "Search ...")
			{
				this.tbSearch.Clear();
				this.tbSearch.ForeColor = Color.Black;
			}
		}

	}
}
