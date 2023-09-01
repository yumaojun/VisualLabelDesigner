using System;
using System.Collections.Generic;
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
using YProgramStudio.LabelsDesigner.Model;

namespace YProgramStudio.LabelsDesigner.Views
{
	/// <summary>
	/// labels打印、打印预览
	/// </summary>
	public partial class PrintPreview : UserControl
	{
		public PrintPreview()
		{
			InitializeComponent();
		}

		/// <summary>
		/// 绘制
		/// </summary>
		public void Renderer(PageRenderer pr)
		{
			labelPreview.Renderer = pr;
		}

		/// <summary>
		/// 打印
		/// </summary>
		private void OnPrintButtonClick(object sender, RoutedEventArgs e)
		{
			PrintDialog printDialog = new PrintDialog();
			if (printDialog.ShowDialog() == true)
			{
				labelPreview.Renderer.Print(null);
				printDialog.PrintVisual(labelPreview, "");
			}
		}
	}
}
