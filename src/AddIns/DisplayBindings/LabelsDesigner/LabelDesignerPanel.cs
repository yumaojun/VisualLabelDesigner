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

		public LabelEditor Editor { get => labelEditor; }

		public event EventHandler LabelWasEdited;

		public LabelDesignerPanel()
		{
			InitializeComponent();

			labelEditor.ZoomChanged += new Labels.ZoomChangeEventHandler(OnZoomChanged);
			
		}

		public LabelDesignerPanel(LabelViewContent vc) : this()
		{
			_viewContent = vc;
		}

		public void ShowFile(Model.Model model)
		{
			_model = model;
			_model.NameChanged += OnNameChanged; // TODO: *增加一些事件处理
			//_model.SelectionChanged += OnSelectionChanged;

			_undoRedoModel = new UndoRedoModel(_model);

			labelEditor.SetModel(_model, _undoRedoModel);


			mainPanel.AutoScrollMinSize = new System.Drawing.Size((int)(_model.Frame.Width), (int)(_model.Frame.Height)); // TODO: *需要加上两边最小边距
		}

		//private void OnSelectionChanged(object sender, EventArgs e)
		//{
		//	//throw new NotImplementedException();
		//}

		// TODO: *Model名称改变
		private void OnNameChanged(object render, EventArgs e)
		{
			Debug.WriteLine("SetModel() OnNameChanged");
		}

		// Size Change Event
		private void LabelDesignerPanel_Resize(object sender, EventArgs e)
		{
			mainPanel.Width = hRuler.Width = Width - origin.Width;
			mainPanel.Height = vRuler.Height = Height - origin.Height;
		}

		private void OnZoomChanged(object render, Labels.ZoomChangeEventArgs e)
		{
			hRuler.UpdateRuler(e.X, e.Y);
			vRuler.UpdateRuler(e.X, e.Y);
		}

		private void LabelDesignerPanel_KeyDown(object sender, KeyEventArgs e)
		{
			ICSharpCode.SharpDevelop.SD.MessageService.ShowMessage(e.KeyData.ToString());
		}
	}
}
