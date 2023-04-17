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
using ZRGPictureBox;

namespace YProgramStudio.LabelsDesigner
{
	public partial class LabelDesignerPanel : UserControl
	{
		ZRGPictureBoxControl pb;

		IViewContent viewContent;

		public event EventHandler LabelWasEdited;

		private LabelDesignerPanel()
		{
			InitializeComponent();
			pb = new ZRGPictureBoxControl();
			pb.Dock = DockStyle.Fill;
			Controls.Add(pb);
		}

		public LabelDesignerPanel(LabelViewContent vc) : this()
		{
			viewContent = vc;
		}
	}
}
