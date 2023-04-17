using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YProgramStudio.ZPLTextEditor.Properties;

namespace YProgramStudio.ZPLTextEditor.Gui
{
	public class PrinterOrCanvasComboBox : System.Windows.Controls.ComboBox
	{
		static PrinterOrCanvasComboBox _instance;
		public static PrinterOrCanvasComboBox Instance
		{
			get
			{
				return _instance;
			}
		}

		List<LabelaryDpmm> labelaryDpmms = new List<LabelaryDpmm>();
		LabelaryDpmm currentLabelaryDpmm;
		List<string> lstRecentPrinter = new List<string>();

		int editIndex = -1;
		int resetIndex = -1;

		public PrinterOrCanvasComboBox()
		{
			_instance = this;
			//LayoutConfiguration.LayoutChanged += new EventHandler(LayoutChanged);
			//SD.ResourceService.LanguageChanged += new EventHandler(ResourceService_LanguageChanged);
			Width = 120;
			CreateRecentPrinter();
		}

		public LabelaryDpmm GetValue()
		{
			return currentLabelaryDpmm;
		}

		void RecreateItems()
		{
			editingLayout = true;
			try
			{
				var comboBox = this;
				comboBox.Items.Clear();
				int index = 0;
				labelaryDpmms = LabelaryDpmm.Data();
				foreach (LabelaryDpmm dpmm in labelaryDpmms)
				{
					comboBox.Items.Add(dpmm);
				}

				editIndex = comboBox.Items.Count;
				resetIndex = comboBox.Items.Count;
				comboBox.SelectedIndex = index;
				currentLabelaryDpmm = labelaryDpmms[index];
			}
			finally
			{
				editingLayout = false;
			}
		}

		void CreateRecentPrinter()
		{
			editingLayout = true;
			try
			{
				var comboBox = this;
				comboBox.Items.Clear();
				int index = 0;
				if (Settings.Default.RecentPrinter != null)
				{
					this.lstRecentPrinter = Settings.Default.RecentPrinter.Cast<string>().ToList<string>();
				}

				foreach (string strPrinter in lstRecentPrinter)
				{
					comboBox.Items.Add(strPrinter);
				}

				editIndex = comboBox.Items.Count;
				resetIndex = comboBox.Items.Count;
				comboBox.SelectedIndex = index;
				//currentLabelaryDpmm = labelaryDpmms[index];//printer
			}
			finally
			{
				editingLayout = false;
			}
		}

		public void RecreateItems(PrinterType printerType)
		{
			if (printerType == null)
			{
				var comboBox = this;
				comboBox.Items.Clear();
				return;
			}
			switch (printerType.Code)
			{
				case "0":
					CreateRecentPrinter();
					break;
				case "1":
					RecreateItems();
					break;
				default:
					break;
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
			currentLabelaryDpmm = labelaryDpmms[index1];
			oldItem = comboBox.SelectedIndex;
		}
	}
}
