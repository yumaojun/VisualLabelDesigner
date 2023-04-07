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

namespace VisualLabelDesigner.ZplLabelDesigner
{
	public partial class ZplLabelDesignerPanel : UserControl
	{
		ZRGPictureBoxControl pb;

		IViewContent viewContent;

		public event EventHandler LabelWasEdited;

		private ZplLabelDesignerPanel()
		{
			InitializeComponent();
			pb = new ZRGPictureBoxControl();
			pb.Dock = DockStyle.Fill;
			Controls.Add(pb);
		}

		public ZplLabelDesignerPanel(ZplLabelViewContent vc) : this()
		{
			viewContent = vc;
		}
	}
}
