using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.ZPLTextEditor.Gui
{
	public class LabelSizeComboBox : System.Windows.Controls.ComboBox
	{
		static LabelSizeComboBox _instance;
		public static LabelSizeComboBox Instance
		{
			get { return _instance; }
		}

		int editIndex = -1;
		int resetIndex = -1;
		private List<LabelFormat> labelFormats = new List<LabelFormat>();
		LabelFormat currentLabelFormat;

		public LabelSizeComboBox()
		{
			_instance = this;
			//LayoutConfiguration.LayoutChanged += new EventHandler(LayoutChanged);
			//SD.ResourceService.LanguageChanged += new EventHandler(ResourceService_LanguageChanged);
			Width = 170;
			RecreateItems();
		}

		public LabelFormat GetValue()
		{
			return currentLabelFormat;
		}

		void RecreateItems()
		{
			editingLayout = true;
			try
			{
				var comboBox = this;
				comboBox.Items.Clear();
				int index = 0;
				labelFormats = LabelFormat.DeserializeFromXML();
				foreach (LabelFormat lf in labelFormats)
				{
					comboBox.Items.Add(lf);
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
			var index1 = comboBox.SelectedIndex == -1 ? 1 : comboBox.SelectedIndex;
			currentLabelFormat = labelFormats[index1];
			oldItem = comboBox.SelectedIndex;
		}
	}
}
