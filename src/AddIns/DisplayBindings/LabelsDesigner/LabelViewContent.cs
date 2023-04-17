using ICSharpCode.SharpDevelop.Workbench;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		public LabelViewContent(OpenedFile file) : base(file)
		{
			designer = new LabelDesignerPanel(this);
			designer.LabelWasEdited += editor_LabelWasEdited;
		}

		// 文件被修改
		void editor_LabelWasEdited(object sender, EventArgs e)
		{
			PrimaryFile.MakeDirty();
		}
	}
}
