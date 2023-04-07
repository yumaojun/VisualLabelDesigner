
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisualLabelDesigner.ZplTextEditor.Services;

namespace VisualLabelDesigner.ZplTextEditor.Gui
{
	public class ZplCommandAssistant : UserControl
	{
		public bool IsEnable
		{
			get
			{
				return this._isEnable;
			}
			set
			{
				this._isEnable = value;
				this.OnSomethingChange(null, EventArgs.Empty);
			}
		}

		public new string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		public string Code
		{
			get
			{
				return this._code;
			}
			set
			{
				this._code = value;
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
				this.OnSomethingChange(null, EventArgs.Empty);
			}
		}

		public ZplCommandAssistant()
		{
			this.InitializeComponent();
		}

		public ZplCommandAssistant ShowParameter(BaseZplParameter _scp, int i, int width)
		{
			ZplCommandAssistant result;
			try
			{
				this.lblName.Text = _scp.Name;
				this.lblDescription.Text = _scp.Description;
				this.lblAcceptedValues.Text = _scp.AcceptedValue;
				base.Location = new Point(2, i * base.Height + 1);
				if (_scp.GetType() == typeof(TextBoxParameter))
				{
					TextBox textBox = new TextBox();
					base.Controls.Add(textBox);
					TextBoxParameter textBoxParameter = (TextBoxParameter)_scp;
					textBox.Text = textBoxParameter.Default;
					textBox.MaxLength = textBoxParameter.Size;
					textBox.TextAlign = HorizontalAlignment.Right;
					textBox.Size = new Size(120, 23);
					textBox.Location = new Point(507, 14);
					textBox.TextChanged += this.Param_TextChanged;
					this._name = _scp.Name;
					this._code = _scp.Code;
					this._isEnable = true;
					this._value = textBoxParameter.Default;
				}
				if (_scp.GetType() == typeof(ComboBoxParameter))
				{
					ComboBox comboBox = new ComboBox();
					base.Controls.Add(comboBox);
					ComboBoxParameter comboBoxParameter = (ComboBoxParameter)_scp;
					comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
					comboBox.DisplayMember = "Content";
					comboBox.ValueMember = "Value";
					comboBox.DataSource = comboBoxParameter.Values;
					comboBox.Size = new Size(120, 23);
					comboBox.DropDownWidth = 220;
					comboBox.Location = new Point(507, 14);
					comboBox.SelectedValue = comboBoxParameter.Default;
					if (comboBox.SelectedIndex == -1)
					{
						comboBox.SelectedIndex = 0;
					}
					comboBox.SelectedIndexChanged += this.Param_TextChanged;
					this._name = comboBoxParameter.Name;
					this._code = _scp.Code;
					this._isEnable = true;
					this._value = comboBox.SelectedValue.ToString();
				}
				if (_scp.GetType() == typeof(NumericBoxParameter))
				{
					NumericUpDown numericUpDown = new NumericUpDown();
					base.Controls.Add(numericUpDown);
					NumericBoxParameter numericBoxParameter = (NumericBoxParameter)_scp;
					numericUpDown.Minimum = numericBoxParameter.Min;
					numericUpDown.Maximum = numericBoxParameter.Max;
					numericUpDown.Increment = numericBoxParameter.Increment;
					numericUpDown.TextAlign = HorizontalAlignment.Right;
					numericUpDown.Size = new Size(120, 23);
					numericUpDown.Location = new Point(507, 14);
					if (!string.IsNullOrEmpty(numericBoxParameter.Default))
					{
						numericUpDown.Value = Convert.ToDecimal(numericBoxParameter.Default.Replace(".", ","));
					}
					numericUpDown.Increment = ((numericBoxParameter.Increment == 0m) ? 1m : numericBoxParameter.Increment);
					if (numericBoxParameter.Increment.ToString() == "0,1")
					{
						numericUpDown.DecimalPlaces = 1;
					}
					numericUpDown.ValueChanged += this.Param_TextChanged;
					this._name = numericBoxParameter.Name;
					this._code = _scp.Code;
					this._isEnable = true;
					this._value = numericUpDown.Text;
				}
				result = this;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message.ToString(), System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				result = this;
			}
			return result;
		}

		private void Param_TextChanged(object sender, EventArgs e)
		{
			if (sender.GetType() == typeof(TextBox))
			{
				this.Value = (sender as TextBox).Text;
			}
			if (sender.GetType() == typeof(NumericUpDown))
			{
				this.Value = (sender as NumericUpDown).Value.ToString();
			}
			if (sender.GetType() == typeof(ComboBox))
			{
				this.Value = (sender as ComboBox).SelectedValue.ToString();
			}
		}

		private void cbEnable_CheckedChanged(object sender, EventArgs e)
		{
			this.lblAcceptedValues.Enabled = this.cbEnable.Checked;
			this.lblDescription.Enabled = this.cbEnable.Checked;
			this.lblName.Enabled = this.cbEnable.Checked;
			foreach (object obj in base.Controls)
			{
				if (obj.GetType() == typeof(NumericUpDown))
				{
					(obj as NumericUpDown).Enabled = this.cbEnable.Checked;
				}
				if (obj.GetType() == typeof(TextBox))
				{
					(obj as TextBox).Enabled = this.cbEnable.Checked;
				}
				if (obj.GetType() == typeof(ComboBox))
				{
					(obj as ComboBox).Enabled = this.cbEnable.Checked;
				}
				this.IsEnable = this.cbEnable.Checked;
			}
		}

		private void OnSomethingChange(object p, EventArgs empty)
		{
			this.SomethingChange(this, empty);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			this.lblName = new Label();
			this.lblDescription = new Label();
			this.lblAcceptedValues = new Label();
			this.cbEnable = new CheckBox();
			base.SuspendLayout();
			this.lblName.Font = new Font("Consolas", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.lblName.Location = new Point(23, 2);
			this.lblName.Name = "lblName";
			this.lblName.Size = new Size(32, 19);
			this.lblName.TabIndex = 0;
			this.lblName.Text = "dat";
			this.lblDescription.Font = new Font("Consolas", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.lblDescription.Location = new Point(50, 2);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Size = new Size(443, 19);
			this.lblDescription.TabIndex = 1;
			this.lblDescription.Text = "Orientation";
			this.lblAcceptedValues.Font = new Font("Consolas", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.lblAcceptedValues.Location = new Point(33, 20);
			this.lblAcceptedValues.Name = "lblAcceptedValues";
			this.lblAcceptedValues.Size = new Size(468, 31);
			this.lblAcceptedValues.TabIndex = 2;
			this.lblAcceptedValues.Text = "label1";
			this.cbEnable.AutoSize = true;
			this.cbEnable.Checked = true;
			this.cbEnable.CheckState = CheckState.Checked;
			this.cbEnable.Location = new Point(4, 4);
			this.cbEnable.Name = "cbEnable";
			this.cbEnable.Size = new Size(15, 14);
			this.cbEnable.TabIndex = 3;
			this.cbEnable.UseVisualStyleBackColor = true;
			this.cbEnable.CheckedChanged += this.cbEnable_CheckedChanged;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = Color.Gainsboro;
			base.Controls.Add(this.cbEnable);
			base.Controls.Add(this.lblAcceptedValues);
			base.Controls.Add(this.lblDescription);
			base.Controls.Add(this.lblName);
			this.Font = new Font("Segoe UI", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Name = "zplAssistant";
			base.Size = new Size(635, 51);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public EventHandler SomethingChange;

		private bool _isEnable;

		private string _name;

		private string _code;

		private string _value;

		private IContainer components;

		private Label lblName;

		private Label lblDescription;

		private Label lblAcceptedValues;

		private CheckBox cbEnable;
	}
}
