using ICSharpCode.SharpDevelop;
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
	public partial class CommandHelper : UserControl, IPadContent
	{
		static CommandHelper _instance;
		public static CommandHelper Instance { get { return _instance; } }

		private Services.ZPLCommand currentZplCommand = new Services.ZPLCommand();

		public CommandHelper()
		{
			_instance = this;
			InitializeComponent();
			if (CommandBox.Instance != null)
			{
				CommandBox.Instance.ZplCommandClick += CommandBox_ZplCommandClick;
			}
			this.cmdParameters1.OpenAssistantClick += ZParameters_OpenAssistantClick;
			// TODO: zplTextPanel释放后，绑定的事件需要移除吗
			ZplTextEditorPanel zplTextPanel = SD.Workbench.ActiveViewContent?.Control as ZplTextEditorPanel;
			if (zplTextPanel != null)
			{
			   this.BtnOkClick += zplTextPanel.Command_BtnOkClick;
			}
		}

		/// <summary>
		/// 订阅CommandBox的按钮事件
		/// </summary>
		/// <param name="cb"></param>
		public void SubscribeCommandClickEvent(CommandBox cb)
		{
			cb.ZplCommandClick += CommandBox_ZplCommandClick;
		}

		private void ztZplCommand_ZplCommandMouseHover(object sender, EventArgs e)
		{
			Services.ZPLCommand zplcommand = (Services.ZPLCommand)((ToolStripButton)sender).Tag;
			if (zplcommand != this.currentZplCommand)
			{
				this.tbBaliseZPL.Text = string.Format("{0} : {1} ({2})", zplcommand.Name, zplcommand.ShortDescription, zplcommand.Category);
				this.tbDescriptionZPL.Text = zplcommand.LongDescription.Replace("\\r\\n", Environment.NewLine);
				this.tbUtilisationZPL.Text = zplcommand.Usage.Replace("\\r\\n", Environment.NewLine);
				this.cmdParameters1.Display(zplcommand);
				this.currentZplCommand = zplcommand;
			}
		}

		private void CommandBox_ZplCommandClick(object sender, EventArgs e)
		{
			Services.ZPLCommand zplcommand = (Services.ZPLCommand)((ToolStripButton)sender).Tag;
			//this.zceCodeZpl.fctbZplCode.SelectedText = zplcommand.Command;
			//this.zceCodeZpl.SetCarretPosition();
			this.ztZplCommand_ZplCommandMouseHover(sender, e);
		}

		public event EventHandler<EventArgsString> BtnOkClick;

		private void ZParameters_OpenAssistantClick(object sender, EventArgsZPLCommand e)
		{
			using (ZplCommandCreate commandCreate = new ZplCommandCreate(e.zplCommand))
			{
				commandCreate.BtnOkClick += CommandCreate_BtnOkClick;
				commandCreate.ShowDialog();
			}
		}

		private void CommandCreate_BtnOkClick(object sender, EventArgsString e)
		{
			if (BtnOkClick != null)
				BtnOkClick(sender, e);
		}

		public object Control => this;

		public object InitiallyFocusedControl => null;

		object IServiceProvider.GetService(Type serviceType)
		{
			return null;
		}


	}
}
