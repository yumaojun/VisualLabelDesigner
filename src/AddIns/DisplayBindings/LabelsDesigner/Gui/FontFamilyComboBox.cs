using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace YProgramStudio.LabelsDesigner.Gui
{
	public class FontFamilyComboBox : System.Windows.Controls.ComboBox
	{
		private List<TextBlock> storeItems = new List<TextBlock>();

		public FontFamilyComboBox()
		{
			Width = 120;
			RecreateItems();
		}

		void RecreateItems()
		{
			foreach (var fontFamily in System.Windows.Media.Fonts.SystemFontFamilies)
			{
				TextBlock textBlock1 = new TextBlock();
				textBlock1.FontFamily = fontFamily;
				textBlock1.Text = fontFamily.Source;
				storeItems.Add(textBlock1);
			}
			ItemsSource = storeItems;
			SelectedValue = storeItems.FirstOrDefault(x => x.Text == "Arial");
		}
	}
}
