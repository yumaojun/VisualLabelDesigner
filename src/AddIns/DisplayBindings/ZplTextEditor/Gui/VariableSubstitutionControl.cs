using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YProgramStudio.ZPLTextEditor.Gui
{
	public partial class VariableSubstitutionControl : UserControl
	{
		private string _name;

		private string _value;

		public new string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
				this.tbName.Text = value;
			}
		}

		public string Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
				this.tbValue.Text = value;
			}
		}

		public VariableSubstitutionControl(string name, string value)
		{
			this.InitializeComponent();
			this._name = name;
			this._value = value;
			this.tbName.DataBindings.Add("Text", this, "Name", false, DataSourceUpdateMode.OnPropertyChanged);
			this.tbValue.DataBindings.Add("Text", this, "Value", false, DataSourceUpdateMode.OnPropertyChanged);
		}

	}
}
