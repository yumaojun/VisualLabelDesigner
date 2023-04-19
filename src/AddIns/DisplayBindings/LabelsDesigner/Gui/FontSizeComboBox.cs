using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Gui
{
	public class FontSizeComboBox : System.Windows.Controls.ComboBox
	{
		static readonly int[] fontSizes = new int[] { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };

		public FontSizeComboBox()
		{
			IsEditable = true;
			Width = 50;
			RecreateItems();
		}

		void RecreateItems()
		{
			foreach (var fontSize in fontSizes)
			{
				Items.Add(fontSize);
			}
			SelectedIndex = 4;
		}
	}
}
