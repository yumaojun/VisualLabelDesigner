
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisualLabelDesigner.ZplTextEditor.Services;

namespace VisualLabelDesigner.ZplTextEditor.Gui
{
	public partial class ZplCommandCreate : Form
	{
		public event EventHandler<EventArgsString> BtnOkClick;

		public ZPLCommand _zplCommand { get; set; }

		public ZplCommandCreate(ZPLCommand zCommand)
		{
			InitializeComponent();
			this._zplCommand = zCommand;
		}

		private void ZplTextCreate_Load(object sender, EventArgs e)
		{
			string title = "Visual Label Designer - Create for {0}: {1}";
			this.Text = string.Format(title, this._zplCommand.Name, this._zplCommand.ShortDescription);
			this.lblUsage.Text = string.Format("Usage: {0}", this._zplCommand.Usage);
			this.tbDescription.Text = this._zplCommand.LongDescription.Replace("\\r\\n", Environment.NewLine);
			int num = 0;
			foreach (BaseZplParameter scp in this._zplCommand.Parameters.ZPLCommandParameters)
			{
				ZplCommandAssistant zAssistant = new ZplCommandAssistant();
				ZplCommandAssistant zAssistant2 = zAssistant.ShowParameter(scp, num, this.splitContainer1.Panel2.Width);
				zAssistant.SomethingChange = (EventHandler)Delegate.Combine(zAssistant.SomethingChange, new EventHandler(this.GenerateZpl));
				num++;
				zAssistant2.Parent = this.splitContainer1.Panel2;
			}
			this.GenerateZpl(null, EventArgs.Empty);
		}

		private void GenerateZpl(object sender, EventArgs e)
		{
			StringBuilder stringBuilder = new StringBuilder(this._zplCommand.Assistant);
			foreach (object obj in this.splitContainer1.Panel2.Controls)
			{
				if (obj.GetType() == typeof(ZplCommandAssistant))
				{
					ZplCommandAssistant zAssistant = obj as ZplCommandAssistant;
					stringBuilder.Replace(zAssistant.Code, (!zAssistant.IsEnable) ? string.Empty : zAssistant.Value.Replace(',', '.'));
				}
			}
			this.lblZplCode.Text = stringBuilder.ToString().TrimEnd(new char[]
			{
				','
			});
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			this.OnBtnOkClick(new EventArgsString(this.lblZplCode.Text));
			base.Close();
		}

		protected virtual void OnBtnOkClick(EventArgsString e)
		{
			this.BtnOkClick(this, e);
		}
	}
}
