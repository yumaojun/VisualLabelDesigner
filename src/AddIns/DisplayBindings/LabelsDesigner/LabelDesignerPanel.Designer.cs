
namespace YProgramStudio.LabelsDesigner
{
	partial class LabelDesignerPanel
	{
		/// <summary> 
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.labelEditor = new YProgramStudio.LabelsDesigner.Gui.LabelEditor();
            this.origin = new YProgramStudio.LabelsDesigner.Gui.Origin();
            this.hRuler = new YProgramStudio.LabelsDesigner.Gui.HRuler();
            this.vRuler = new YProgramStudio.LabelsDesigner.Gui.VRuler();
            this.tableLayoutPanel1.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.mainPanel, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.origin, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.hRuler, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.vRuler, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(850, 540);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // mainPanel
            // 
            this.mainPanel.AutoScroll = true;
            this.mainPanel.Controls.Add(this.labelEditor);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(24, 24);
            this.mainPanel.Margin = new System.Windows.Forms.Padding(0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(826, 1174);
            this.mainPanel.TabIndex = 0;
            // 
            // labelEditor
            // 
            this.labelEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEditor.Location = new System.Drawing.Point(0, 0);
            this.labelEditor.Margin = new System.Windows.Forms.Padding(0);
            this.labelEditor.Name = "labelEditor";
            this.labelEditor.Size = new System.Drawing.Size(826, 1174);
            this.labelEditor.TabIndex = 0;
            this.labelEditor.Text = "labelEditor1";
            // 
            // origin
            // 
            this.origin.BackColor = System.Drawing.Color.Lavender;
            this.origin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.origin.Location = new System.Drawing.Point(0, 0);
            this.origin.Margin = new System.Windows.Forms.Padding(0);
            this.origin.Name = "origin";
            this.origin.Size = new System.Drawing.Size(24, 24);
            this.origin.TabIndex = 1;
            this.origin.Text = "origin";
            // 
            // hRuler
            // 
            this.hRuler.BackColor = System.Drawing.Color.Lavender;
            this.hRuler.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hRuler.Location = new System.Drawing.Point(24, 0);
            this.hRuler.Margin = new System.Windows.Forms.Padding(0);
            this.hRuler.MaximumSize = new System.Drawing.Size(0, 28);
            this.hRuler.Name = "hRuler";
            this.hRuler.Size = new System.Drawing.Size(826, 24);
            this.hRuler.TabIndex = 2;
            this.hRuler.Text = "hRuler";
            this.hRuler.X0 = 0F;
            this.hRuler.Y0 = 0F;
            // 
            // vRuler
            // 
            this.vRuler.BackColor = System.Drawing.Color.Lavender;
            this.vRuler.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vRuler.Location = new System.Drawing.Point(0, 24);
            this.vRuler.Margin = new System.Windows.Forms.Padding(0);
            this.vRuler.Name = "vRuler";
            this.vRuler.Size = new System.Drawing.Size(24, 1174);
            this.vRuler.TabIndex = 3;
            this.vRuler.Text = "vRuler";
            this.vRuler.X0 = 0F;
            this.vRuler.Y0 = 0F;
            // 
            // LabelDesignerPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(45, 24, 45, 24);
            this.Name = "LabelDesignerPanel";
            this.Size = new System.Drawing.Size(850, 540);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LabelDesignerPanel_KeyDown);
            this.Resize += new System.EventHandler(this.LabelDesignerPanel_Resize);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.mainPanel.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Panel mainPanel;
		private Gui.Origin origin;
		private Gui.HRuler hRuler;
		private Gui.VRuler vRuler;
		private Gui.LabelEditor labelEditor;
	}
}
