using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Gui
{
	public class FontFamilyComboBox : System.Windows.Controls.ComboBox
	{
		public FontFamilyComboBox()
		{
			Width = 120;
			RecreateItems();
		}

		void RecreateItems()
		{
			InstalledFontCollection fonts = new InstalledFontCollection();
			foreach (var fontFamily in fonts.Families)
			{
				Items.Add(fontFamily.Name);
			}
			SelectedValue = "Arial";
		}
	}
}
