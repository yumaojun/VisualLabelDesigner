using ICSharpCode.Core;
using ICSharpCode.SharpDevelop.Workbench;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YProgramStudio.LabelsDesigner.Gui;
using YProgramStudio.LabelsDesigner.Model;

namespace YProgramStudio.LabelsDesigner
{
	public partial class LabelDesignerPanel : UserControl
	{
		private Model.Model _model;
		private UndoRedoModel _undoRedoModel;

		private IViewContent _viewContent;

		public event EventHandler LabelWasEdited;

		public LabelDesignerPanel()
		{
			InitializeComponent();
		}

		public LabelDesignerPanel(LabelViewContent vc) : this()
		{
			_viewContent = vc;
		}

		public void ShowFile(Model.Model model)
		{
			_model = model;
			_model.NameChanged += OnNameChanged; // TODO: *增加一些事件处理

			_undoRedoModel = new UndoRedoModel(_model);

			labelEditor.SetModel(_model, _undoRedoModel);
			mainPanel.AutoScrollMinSize = new System.Drawing.Size((int)(_model.Frame.Width), (int)(_model.Frame.Height)); // TODO: *需要加上两边最小边距
		}

		// TODO: *Model名称改变
		private void OnNameChanged(object render, EventArgs e)
		{
			Debug.WriteLine("SetModel() OnNameChanged");
		}

		private void LabelDesignerPanel_Resize(object sender, EventArgs e)
		{
			mainPanel.Width = hRuler.Width = Width - origin.Width;
			mainPanel.Height = vRuler.Height = Height - origin.Height;
		}
	}
}
