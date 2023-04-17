using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YProgramStudio.ZPLTextEditor.Properties;
using YProgramStudio.ZPLTextEditor.Services;

namespace YProgramStudio.ZPLTextEditor.Gui
{
	public partial class VariableMappingForm : Form
	{
		private List<string> _listVariables = new List<string>();

		private Dictionary<string, string> _dicVariableSubstitution = new Dictionary<string, string>();

		public VariableMappingForm(string zplcode, Dictionary<string, string> variable)
		{
			this._dicVariableSubstitution = variable;
			this._listVariables = ZPLCode.GetVariables(zplcode, Settings.Default.VariableStartChars, Settings.Default.VariableEndChars);
			InitializeComponent();
			this.lblNoVariable.Visible = (this._listVariables.Count == 0);
		}

		private void VariableMappingForm_Load(object sender, EventArgs e)
		{
			this.lblDescription.Text = string.Format("Variable substitution will automatically detect and replace variable on your ZPL code when you want to preview a label. According to you current configuration, a variable start with {0} and end with {1}.\r\n\r\nGo to File / Preferences menu to change these settings.", Settings.Default.VariableStartChars, Settings.Default.VariableEndChars);
			int num = 0;
			foreach (string text in this._listVariables)
			{
				string value;
				this._dicVariableSubstitution.TryGetValue(string.Format("{0}{1}{2}", Settings.Default.VariableStartChars, text, Settings.Default.VariableEndChars), out value);
				VariableSubstitutionControl zVariableSubstitution = new VariableSubstitutionControl(text, value);
				zVariableSubstitution.Location = new Point(10, num * 80);
				zVariableSubstitution.Parent = this.splitContainer1.Panel2;
				num++;
			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			try
			{
				foreach (VariableSubstitutionControl zVariableSubstitution in this.splitContainer1.Panel2.Controls.OfType<VariableSubstitutionControl>())
				{
					if (!string.IsNullOrEmpty(zVariableSubstitution.Name) && !string.IsNullOrEmpty(zVariableSubstitution.Value))
					{
						this._dicVariableSubstitution[string.Format("{0}{1}{2}", Settings.Default.VariableStartChars, zVariableSubstitution.Name, Settings.Default.VariableEndChars)] = zVariableSubstitution.Value;
					}
				}
				if (!Directory.Exists(VarsFile.ApplicationDirectory()))
				{
					Directory.CreateDirectory(VarsFile.ApplicationDirectory());
				}
				string path = VarsFile.VariableSubstitution();
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				using (StreamWriter streamWriter = new StreamWriter(path))
				{
					foreach (KeyValuePair<string, string> keyValuePair in this._dicVariableSubstitution)
					{
						streamWriter.WriteLine("{0}\t{1}", keyValuePair.Key, ZPLCode.Clean(keyValuePair.Value));
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			finally
			{
				base.Close();
			}
		}
	}
}
