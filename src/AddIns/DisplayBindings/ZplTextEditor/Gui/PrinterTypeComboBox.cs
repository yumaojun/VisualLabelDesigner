using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.ZPLTextEditor.Gui
{
	public class PrinterTypeComboBox : System.Windows.Controls.ComboBox
	{
		static PrinterTypeComboBox _instance;
		public static PrinterTypeComboBox Instance
		{
			get
			{
				return _instance;
			}
		}

		List<PrinterType> printerTypes = new List<PrinterType>();
		PrinterType currentPrinterType;

		int editIndex = -1;
		int resetIndex = -1;

		public PrinterTypeComboBox()
		{
			_instance = this;
			//LayoutConfiguration.LayoutChanged += new EventHandler(LayoutChanged);
			//SD.ResourceService.LanguageChanged += new EventHandler(ResourceService_LanguageChanged);
			Width = 120;
			RecreateItems();
		}

		public PrinterType GetValue()
		{
			return currentPrinterType;
		}

		void RecreateItems()
		{
			editingLayout = true;
			try
			{
				var comboBox = this;
				comboBox.Items.Clear();
				int index = 0;
				printerTypes = PrinterType.Data();
				foreach (PrinterType dpmm in printerTypes)
				{
					comboBox.Items.Add(dpmm);
				}

				editIndex = comboBox.Items.Count;
				resetIndex = comboBox.Items.Count;
				comboBox.SelectedIndex = index;
			}
			finally
			{
				editingLayout = false;
			}
		}

		int oldItem = 0;
		bool editingLayout;

		protected override void OnSelectionChanged(System.Windows.Controls.SelectionChangedEventArgs e)
		{
			base.OnSelectionChanged(e);

			if (editingLayout) return;

			var comboBox = this;
			var index1 = comboBox.SelectedIndex == -1 ? 0 : comboBox.SelectedIndex;
			currentPrinterType = printerTypes[index1];
			oldItem = comboBox.SelectedIndex;
			PrinterOrCanvasComboBox.Instance.RecreateItems(currentPrinterType);
		}
	}
}
