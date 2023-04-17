
namespace YProgramStudio.ZPLTextEditor.Gui
{
	partial class CommandHelper
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		internal System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
		internal System.Windows.Forms.Label label1;
		internal System.Windows.Forms.TextBox tbUtilisationZPL;
		internal System.Windows.Forms.Label lblUtilisationZPL;
		internal System.Windows.Forms.TextBox tbDescriptionZPL;
		internal System.Windows.Forms.Label lblBaliseZPL;
		internal System.Windows.Forms.TextBox tbBaliseZPL;
		internal System.Windows.Forms.Label lblDescriptionZPL;
		internal CommandParameters cmdParameters1;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region 组件设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要修改
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.tbUtilisationZPL = new System.Windows.Forms.TextBox();
            this.lblUtilisationZPL = new System.Windows.Forms.Label();
            this.tbDescriptionZPL = new System.Windows.Forms.TextBox();
            this.lblBaliseZPL = new System.Windows.Forms.Label();
            this.tbBaliseZPL = new System.Windows.Forms.TextBox();
            this.lblDescriptionZPL = new System.Windows.Forms.Label();
            this.cmdParameters1 = new ZPLTextEditor.Gui.CommandParameters();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.label1, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.tbUtilisationZPL, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.lblUtilisationZPL, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.tbDescriptionZPL, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.lblBaliseZPL, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.tbBaliseZPL, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.lblDescriptionZPL, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.cmdParameters1, 1, 3);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 4;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(150, 150);
            this.tableLayoutPanel.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 121);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "Parameters :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tbUtilisationZPL
            // 
            this.tbUtilisationZPL.BackColor = System.Drawing.Color.Gainsboro;
            this.tbUtilisationZPL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbUtilisationZPL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbUtilisationZPL.Location = new System.Drawing.Point(120, 90);
            this.tbUtilisationZPL.Multiline = true;
            this.tbUtilisationZPL.Name = "tbUtilisationZPL";
            this.tbUtilisationZPL.ReadOnly = true;
            this.tbUtilisationZPL.Size = new System.Drawing.Size(27, 25);
            this.tbUtilisationZPL.TabIndex = 4;
            // 
            // lblUtilisationZPL
            // 
            this.lblUtilisationZPL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUtilisationZPL.AutoSize = true;
            this.lblUtilisationZPL.Location = new System.Drawing.Point(51, 90);
            this.lblUtilisationZPL.Margin = new System.Windows.Forms.Padding(3);
            this.lblUtilisationZPL.Name = "lblUtilisationZPL";
            this.lblUtilisationZPL.Size = new System.Drawing.Size(63, 15);
            this.lblUtilisationZPL.TabIndex = 4;
            this.lblUtilisationZPL.Text = "Usage :";
            this.lblUtilisationZPL.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tbDescriptionZPL
            // 
            this.tbDescriptionZPL.BackColor = System.Drawing.Color.Gainsboro;
            this.tbDescriptionZPL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbDescriptionZPL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbDescriptionZPL.Location = new System.Drawing.Point(120, 34);
            this.tbDescriptionZPL.Multiline = true;
            this.tbDescriptionZPL.Name = "tbDescriptionZPL";
            this.tbDescriptionZPL.ReadOnly = true;
            this.tbDescriptionZPL.Size = new System.Drawing.Size(27, 50);
            this.tbDescriptionZPL.TabIndex = 3;
            // 
            // lblBaliseZPL
            // 
            this.lblBaliseZPL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBaliseZPL.AutoSize = true;
            this.lblBaliseZPL.Location = new System.Drawing.Point(27, 3);
            this.lblBaliseZPL.Margin = new System.Windows.Forms.Padding(3);
            this.lblBaliseZPL.Name = "lblBaliseZPL";
            this.lblBaliseZPL.Size = new System.Drawing.Size(87, 15);
            this.lblBaliseZPL.TabIndex = 0;
            this.lblBaliseZPL.Text = "ZPL code :";
            this.lblBaliseZPL.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tbBaliseZPL
            // 
            this.tbBaliseZPL.BackColor = System.Drawing.Color.Gainsboro;
            this.tbBaliseZPL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbBaliseZPL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbBaliseZPL.Location = new System.Drawing.Point(120, 3);
            this.tbBaliseZPL.Multiline = true;
            this.tbBaliseZPL.Name = "tbBaliseZPL";
            this.tbBaliseZPL.ReadOnly = true;
            this.tbBaliseZPL.Size = new System.Drawing.Size(27, 25);
            this.tbBaliseZPL.TabIndex = 1;
            // 
            // lblDescriptionZPL
            // 
            this.lblDescriptionZPL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDescriptionZPL.AutoSize = true;
            this.lblDescriptionZPL.Location = new System.Drawing.Point(3, 34);
            this.lblDescriptionZPL.Margin = new System.Windows.Forms.Padding(3);
            this.lblDescriptionZPL.Name = "lblDescriptionZPL";
            this.lblDescriptionZPL.Size = new System.Drawing.Size(111, 15);
            this.lblDescriptionZPL.TabIndex = 2;
            this.lblDescriptionZPL.Text = "Description :";
            this.lblDescriptionZPL.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cmdParameters1
            // 
            this.cmdParameters1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cmdParameters1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmdParameters1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdParameters1.Location = new System.Drawing.Point(121, 122);
            this.cmdParameters1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmdParameters1.Name = "cmdParameters1";
            this.cmdParameters1.Size = new System.Drawing.Size(25, 42);
            this.cmdParameters1.TabIndex = 6;
            // 
            // CommandHelper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "CommandHelper";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion
	}
}
