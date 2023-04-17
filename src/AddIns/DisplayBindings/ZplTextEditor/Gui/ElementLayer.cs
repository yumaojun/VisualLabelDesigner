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

namespace YProgramStudio.ZPLTextEditor.Gui
{
	public partial class ElementLayer : UserControl, IPadContent
	{
		public ElementLayer()
		{
			InitializeComponent();
		}

		public object Control =>  this;

		public object InitiallyFocusedControl => null;

		object IServiceProvider.GetService(Type serviceType)
		{
			return null;
		}
	}
}
