
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisualLabelDesigner.ZplTextEditor.Services;

namespace VisualLabelDesigner.ZplTextEditor.Gui
{
	public class CommandParameters : UserControl
	{
		private System.ComponentModel.IContainer components;

		private Button btnCreateAssistant;

		private TableLayoutPanel tableLayoutPanel1;

		private RichTextBox rtbParameterDescription;

		public event EventHandler<EventArgsZPLCommand> OpenAssistantClick;

		public CommandParameters()
		{
			this.InitializeComponent();
			this.rtbParameterDescription.SelectionTabs = new int[]
			{
				30,
				250
			};
		}

		public void Display(Services.ZPLCommand zplCommand)
		{
			int num = 1;
			this.rtbParameterDescription.Clear();
			this.btnCreateAssistant.Enabled = true;
			if (zplCommand.Parameters != null && zplCommand.Parameters.ZPLCommandParameters.Count<BaseZplParameter>() > 0)
			{
				this.rtbParameterDescription.SelectionFont = new Font(this.rtbParameterDescription.Font, FontStyle.Bold);
				this.rtbParameterDescription.AppendText("Prm\tDescription\tAccepted Values\r\n");
				this.rtbParameterDescription.SelectionFont = new Font(this.rtbParameterDescription.Font, FontStyle.Regular);
				using (List<BaseZplParameter>.Enumerator enumerator = zplCommand.Parameters.ZPLCommandParameters.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						BaseZplParameter baseZplParameter = enumerator.Current;
						this.rtbParameterDescription.AppendText("-------------------------------------------------------------------------------------------------------------------------\r\n");
						this.rtbParameterDescription.AppendText(string.Format(" {0}\t{1}", baseZplParameter.Name, baseZplParameter.Description));
						if (num < baseZplParameter.Description.Length)
						{
							num = baseZplParameter.Description.Length;
						}
						if (baseZplParameter.GetType() == typeof(ComboBoxParameter))
						{
							ComboBoxParameter comboBoxParameter = (ComboBoxParameter)baseZplParameter;
							bool flag = true;
							using (List<ComboBoxValue>.Enumerator enumerator2 = comboBoxParameter.Values.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									ComboBoxValue comboBoxValue = enumerator2.Current;
									this.rtbParameterDescription.AppendText(string.Format("{0}\t- {1} = {2}\r\n", flag ? string.Empty : "\t", comboBoxValue.Value, comboBoxValue.Content));
									flag = false;
								}
								continue;
							}
						}
						this.rtbParameterDescription.AppendText(string.Format("\t{0}\r\n", baseZplParameter.AcceptedValue));
					}
					goto IL_1DC;
				}
			}
			this.rtbParameterDescription.AppendText("-------------------------------------------------------------------------------------------------------------------------\r\n");
			this.rtbParameterDescription.AppendText(string.Format("-- No parameters available for {0} command.\r\n", zplCommand.Name));
			this.btnCreateAssistant.Enabled = false;
		IL_1DC:
			this.rtbParameterDescription.AppendText("-------------------------------------------------------------------------------------------------------------------------");
			this.btnCreateAssistant.Text = zplCommand.Name + "\r\n Create";
			this.btnCreateAssistant.Tag = zplCommand;
		}

		private void btnCreateAssistant_Click(object sender, EventArgs e)
		{
			ZPLCommand zc = (sender as Button).Tag as ZPLCommand;
			this.OnOpenAssistantClick(new EventArgsZPLCommand(zc));
		}

		protected virtual void OnOpenAssistantClick(EventArgsZPLCommand e)
		{
			this.OpenAssistantClick(this, e);
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
			this.btnCreateAssistant = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rtbParameterDescription = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCreateAssistant
            // 
            this.btnCreateAssistant.Enabled = false;
            this.btnCreateAssistant.Location = new System.Drawing.Point(104, 3);
            this.btnCreateAssistant.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCreateAssistant.Name = "btnCreateAssistant";
            this.btnCreateAssistant.Size = new System.Drawing.Size(92, 48);
            this.btnCreateAssistant.TabIndex = 0;
            this.btnCreateAssistant.Text = "Create";
            this.btnCreateAssistant.UseVisualStyleBackColor = true;
            this.btnCreateAssistant.Click += new System.EventHandler(this.btnCreateAssistant_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.Controls.Add(this.btnCreateAssistant, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.rtbParameterDescription, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 173);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // rtbParameterDescription
            // 
            this.rtbParameterDescription.BackColor = System.Drawing.Color.Gainsboro;
            this.rtbParameterDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbParameterDescription.DetectUrls = false;
            this.rtbParameterDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbParameterDescription.Location = new System.Drawing.Point(4, 3);
            this.rtbParameterDescription.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rtbParameterDescription.Name = "rtbParameterDescription";
            this.rtbParameterDescription.ReadOnly = true;
            this.rtbParameterDescription.Size = new System.Drawing.Size(92, 167);
            this.rtbParameterDescription.TabIndex = 1;
            this.rtbParameterDescription.Text = "";
            this.rtbParameterDescription.WordWrap = false;
            // 
            // CommandParameters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "CommandParameters";
            this.Size = new System.Drawing.Size(200, 173);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
	}
}
