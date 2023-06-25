using ICSharpCode.Core;
using ICSharpCode.SharpDevelop;
using ICSharpCode.SharpDevelop.Workbench;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YProgramStudio.LabelsDesigner.Model;

namespace YProgramStudio.LabelsDesigner
{
	public class LabelViewContent : AbstractViewContent
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
				Db.Init(); // TODO: *后续需要移到程序启动时加载
				Backends.Barcode.Backends.Init();
				Barcodes.Factory.Init();

				Model.Model model = XmlLabelParser.ReadFile(file, stream);
				if (model != null)
				{
					designer.ShowFile(model);
					Gui.BarcodeProperty.Instance.SetModel(model, null);
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
	}
}
