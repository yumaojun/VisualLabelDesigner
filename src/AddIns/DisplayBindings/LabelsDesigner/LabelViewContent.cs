using ICSharpCode.Core;
using ICSharpCode.SharpDevelop;
using ICSharpCode.SharpDevelop.WinForms;
using ICSharpCode.SharpDevelop.Workbench;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YProgramStudio.LabelsDesigner.Core;
using YProgramStudio.LabelsDesigner.Gui;
using YProgramStudio.LabelsDesigner.Model;

namespace YProgramStudio.LabelsDesigner
{
	/// <summary>
	/// TODO: 需要实现：打印、打印预览，保存、另存为，撤销、重做，剪切、复制、粘贴、删除，右键菜单，快捷键
	/// IPrintable：打印
	/// IUndoHandler：撤销、重做
	/// IClipboardHandler：剪切、复制、粘贴、删除
	/// 右键菜单
	/// 快捷键
	/// </summary>
	public class LabelViewContent : AbstractViewContent, IClipboardHandler, IUndoHandler, IPrintable, ILabelOperable
	{
		LabelDesignerPanel designer;

		public override object Control
		{
			get
			{
				return designer;
			}
		}

		/// <summary>
		/// TODO：InitiallyFocusedControl这个是干嘛的，有什么作用？
		/// </summary>
		public override object InitiallyFocusedControl
		{
			get { return designer.Editor; }
		}

		public bool EnableUndo => true;

		public bool EnableRedo => true;

		public bool EnableCut => true;

		public bool EnableCopy => true;

		public bool EnablePaste => true;

		public bool EnableDelete => true;

		public bool EnableSelectAll => true;

		public PrintDocument PrintDocument => throw new NotImplementedException();

		public bool IsAnySelected
		{
			get
			{
				var ldp = Control as LabelDesignerPanel;
				var editor = ldp.Editor as LabelEditor;
				var obj = editor.Model.GetFirstSelectedObject();
				return obj != null;
			}
		}

		public bool IsMultiSelected
		{
			get
			{
				var ldp = Control as LabelDesignerPanel;
				var editor = ldp.Editor as LabelEditor;
				var obj = editor.Model.GetFirstSelectedObject();
				return obj != null;
			}
		}

		public LabelViewContent(OpenedFile file) : base(file)
		{
			//Template template = CreateOrLoad();
			//Frame frame = template.Frames.FirstOrDefault();
			//Model.Model model = new Model.Model();
			//model.Template = template;
			//model.Rotate = frame.H() > frame.W();
			//model.Modified = false;
			designer = new LabelDesignerPanel(this);
			designer.LabelWasEdited += editor_LabelWasEdited;
			//designer.KeyDown += Designer_KeyDown;
		}

		//private void Designer_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		//{
		//	ICSharpCode.SharpDevelop.SD.MessageService.ShowMessage(e.KeyData.ToString() + "(1)");
		//}

		public override void Load(OpenedFile file, Stream stream)
		{
			try
			{
				//
				// Initialize subsystems
				//
				//glabels::model::Settings::init();
				//glabels::model::Db::init();
				//glabels::merge::Factory::init();
				//glabels::barcode::Backends::init();
				//Db.Init(); // TODO: *后续需要移到程序启动时加载
				Backends.Barcode.Backends.Init();
				//Barcodes.Factory.Init();

				Model.Model model = XmlLabelParser.ReadFile(file, stream);
				if (model != null)
				{
					designer.ShowFile(model);
					Views.ObjectProperties.Instance?.SetModel(model, null);
				}
				else
				{
					MessageService.ShowMessage($"Failed to load file. {file.FileName}");
				}
			}
			catch (Exception ex)
			{
				SD.MainThread.InvokeAsyncAndForget(delegate
				{
					MessageService.ShowHandledException(ex);
					if (WorkbenchWindow != null)
					{
						WorkbenchWindow.CloseWindow(true);
					}
				});
			}
		}

		// 文件被修改
		void editor_LabelWasEdited(object sender, EventArgs e)
		{
			PrimaryFile.MakeDirty();
		}

		Template CreateOrLoad()
		{
			Template template = new Template();
			return template;
		}

		public override void Save(OpenedFile file, Stream stream)
		{
		}

		public void Undo()
		{
			SD.MessageService.ShowMessage("Undo");
		}

		public void Redo()
		{
			SD.MessageService.ShowMessage("Redo");
		}

		public void Cut()
		{
			SD.MessageService.ShowMessage("Cut");
		}

		public void Copy()
		{
			SD.MessageService.ShowMessage("Copy");
		}

		public void Paste()
		{
			SD.MessageService.ShowMessage("Paste");
		}

		public void Delete()
		{
			SD.MessageService.ShowMessage("Delete");
		}

		public void SelectAll()
		{
			SD.MessageService.ShowMessage("SelectAll");
		}
	}
}
