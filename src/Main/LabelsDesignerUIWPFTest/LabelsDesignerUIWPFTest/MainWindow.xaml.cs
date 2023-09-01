using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LabelsDesignerUIWPFTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
			Init();
		}

		private void Init()
		{
			//YProgramStudio.LabelsDesigner.Model.Db.Init(); // TODO: *后续需要移到程序启动时加载
			YProgramStudio.LabelsDesigner.Backends.Barcode.Backends.Init();
			//YProgramStudio.LabelsDesigner.Barcodes.Factory.Init();

			string fileName = "hello.glabels";
			FileStream stream = File.OpenRead("D:\\rect.glabels");

			YProgramStudio.LabelsDesigner.Model.Model model = YProgramStudio.LabelsDesigner.Model.XmlLabelParser.ReadFile(fileName, stream);

			YProgramStudio.LabelsDesigner.Model.PageRenderer pr = new YProgramStudio.LabelsDesigner.Model.PageRenderer();

			pr.Model = model;

			printPreview.Renderer(pr);
		}
    }
}
