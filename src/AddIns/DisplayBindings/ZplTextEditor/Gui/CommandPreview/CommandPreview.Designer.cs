
namespace VisualLabelDesigner.ZplTextEditor.Gui
{
	partial class CommandPreview
	{
		/// <summary> 
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		private System.Windows.Forms.ToolStrip toolStrip1;

		private System.Windows.Forms.ToolStripLabel toolStripLabel1;

		private System.Windows.Forms.ToolStripComboBox cbZoom;

		private System.Windows.Forms.ToolStripButton btnZoomPlus;

		private System.Windows.Forms.ToolStripButton btnZoomMinus;

		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;

		private System.Windows.Forms.ToolStripButton btnRotateLeft;

		private System.Windows.Forms.ToolStripButton btnRotateRight;

		private System.Windows.Forms.Panel panel1;

		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;

		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;

		private System.Windows.Forms.ToolStripButton btnCopyImage;

		private System.Windows.Forms.ToolStripButton btnSaveImage;

		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;

		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		private System.Windows.Forms.ToolStripMenuItem tsmiCopyImage;

		private System.Windows.Forms.ToolStripMenuItem tsmiSaveImage;

		private System.Windows.Forms.ToolStripButton btnPreviousLabel;

		private System.Windows.Forms.ToolStripLabel lblLabelPosition;

		private System.Windows.Forms.ToolStripButton btnNextLabel;

		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;

		private System.Windows.Forms.ToolStripLabel lblOrientation;

		private System.Windows.Forms.ToolStripButton tsbAutoCrop;

		private System.Windows.Forms.PictureBox pbPreview;

		private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommandPreview));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnZoomPlus = new System.Windows.Forms.ToolStripButton();
            this.btnZoomMinus = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.cbZoom = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRotateLeft = new System.Windows.Forms.ToolStripButton();
            this.lblOrientation = new System.Windows.Forms.ToolStripLabel();
            this.btnRotateRight = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnPreviousLabel = new System.Windows.Forms.ToolStripButton();
            this.lblLabelPosition = new System.Windows.Forms.ToolStripLabel();
            this.btnNextLabel = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.btnCopyImage = new System.Windows.Forms.ToolStripButton();
            this.btnSaveImage = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbAutoCrop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pbPreview = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiCopyImage = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSaveImage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(18, 18);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnZoomPlus,
            this.btnZoomMinus,
            this.toolStripSeparator2,
            this.toolStripLabel1,
            this.cbZoom,
            this.toolStripSeparator1,
            this.btnRotateLeft,
            this.lblOrientation,
            this.btnRotateRight,
            this.toolStripSeparator3,
            this.btnPreviousLabel,
            this.lblLabelPosition,
            this.btnNextLabel,
            this.toolStripSeparator5,
            this.btnCopyImage,
            this.btnSaveImage,
            this.toolStripSeparator4,
            this.tsbAutoCrop,
            this.toolStripSeparator6});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(584, 31);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnZoomPlus
            // 
            this.btnZoomPlus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZoomPlus.Image = global::VisualLabelDesigner.ZplTextEditor.Properties.Resources.ic_zoom_plus;
            this.btnZoomPlus.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoomPlus.Name = "btnZoomPlus";
            this.btnZoomPlus.Size = new System.Drawing.Size(29, 28);
            this.btnZoomPlus.Text = "Zoom +";
            this.btnZoomPlus.Click += new System.EventHandler(this.btnZoomPlus_Click);
            // 
            // btnZoomMinus
            // 
            this.btnZoomMinus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZoomMinus.Image = global::VisualLabelDesigner.ZplTextEditor.Properties.Resources.ic_zoom_minus;
            this.btnZoomMinus.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoomMinus.Name = "btnZoomMinus";
            this.btnZoomMinus.Size = new System.Drawing.Size(29, 28);
            this.btnZoomMinus.Text = "Zoom -";
            this.btnZoomMinus.Click += new System.EventHandler(this.btnZoomMinus_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(54, 28);
            this.toolStripLabel1.Text = "Zoom:";
            // 
            // cbZoom
            // 
            this.cbZoom.AutoSize = false;
            this.cbZoom.DropDownWidth = 50;
            this.cbZoom.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cbZoom.Items.AddRange(new object[] {
            "300%",
            "250%",
            "200%",
            "175%",
            "150%",
            "125%",
            "100%",
            "75%",
            "66%",
            "50%",
            "33%",
            "25%",
            "16%",
            "12%",
            "8%",
            "6%",
            "5%",
            "4%",
            "3%",
            "2%",
            "1%"});
            this.cbZoom.Name = "cbZoom";
            this.cbZoom.Size = new System.Drawing.Size(85, 28);
            this.cbZoom.SelectedIndexChanged += new System.EventHandler(this.cbZoom_SelectedIndexChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // btnRotateLeft
            // 
            this.btnRotateLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRotateLeft.Image = global::VisualLabelDesigner.ZplTextEditor.Properties.Resources.ic_rotate_left;
            this.btnRotateLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRotateLeft.Name = "btnRotateLeft";
            this.btnRotateLeft.Size = new System.Drawing.Size(29, 28);
            this.btnRotateLeft.Text = "Rotate -90°";
            // 
            // lblOrientation
            // 
            this.lblOrientation.AutoSize = false;
            this.lblOrientation.Name = "lblOrientation";
            this.lblOrientation.Size = new System.Drawing.Size(30, 22);
            this.lblOrientation.Text = "0°";
            // 
            // btnRotateRight
            // 
            this.btnRotateRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRotateRight.Image = global::VisualLabelDesigner.ZplTextEditor.Properties.Resources.ic_rotate_right;
            this.btnRotateRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRotateRight.Name = "btnRotateRight";
            this.btnRotateRight.Size = new System.Drawing.Size(29, 28);
            this.btnRotateRight.Text = "Rotate 90°";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 31);
            // 
            // btnPreviousLabel
            // 
            this.btnPreviousLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPreviousLabel.Enabled = false;
            this.btnPreviousLabel.Image = ((System.Drawing.Image)(resources.GetObject("btnPreviousLabel.Image")));
            this.btnPreviousLabel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPreviousLabel.Name = "btnPreviousLabel";
            this.btnPreviousLabel.Size = new System.Drawing.Size(29, 28);
            this.btnPreviousLabel.Text = "toolStripButton1";
            this.btnPreviousLabel.ToolTipText = "Previous label";
            this.btnPreviousLabel.Click += new System.EventHandler(this.btnChangeLabel_Click);
            // 
            // lblLabelPosition
            // 
            this.lblLabelPosition.Name = "lblLabelPosition";
            this.lblLabelPosition.Size = new System.Drawing.Size(33, 28);
            this.lblLabelPosition.Text = "0/0";
            this.lblLabelPosition.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnNextLabel
            // 
            this.btnNextLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnNextLabel.Enabled = false;
            this.btnNextLabel.Image = ((System.Drawing.Image)(resources.GetObject("btnNextLabel.Image")));
            this.btnNextLabel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNextLabel.Name = "btnNextLabel";
            this.btnNextLabel.Size = new System.Drawing.Size(29, 28);
            this.btnNextLabel.Text = "toolStripButton2";
            this.btnNextLabel.ToolTipText = "Next label";
            this.btnNextLabel.Click += new System.EventHandler(this.btnChangeLabel_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 31);
            // 
            // btnCopyImage
            // 
            this.btnCopyImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCopyImage.Image = global::VisualLabelDesigner.ZplTextEditor.Properties.Resources.ic_copy;
            this.btnCopyImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCopyImage.Name = "btnCopyImage";
            this.btnCopyImage.Size = new System.Drawing.Size(29, 28);
            this.btnCopyImage.Text = "Copy image";
            this.btnCopyImage.Click += new System.EventHandler(this.btnCopyImage_Click);
            // 
            // btnSaveImage
            // 
            this.btnSaveImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSaveImage.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveImage.Image")));
            this.btnSaveImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveImage.Name = "btnSaveImage";
            this.btnSaveImage.Size = new System.Drawing.Size(29, 28);
            this.btnSaveImage.Text = "Save Image As";
            this.btnSaveImage.ToolTipText = "Save Image As ...";
            this.btnSaveImage.Click += new System.EventHandler(this.tsmiCopyImage_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 31);
            // 
            // tsbAutoCrop
            // 
            this.tsbAutoCrop.Checked = true;
            this.tsbAutoCrop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbAutoCrop.Image = ((System.Drawing.Image)(resources.GetObject("tsbAutoCrop.Image")));
            this.tsbAutoCrop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAutoCrop.Name = "tsbAutoCrop";
            this.tsbAutoCrop.Size = new System.Drawing.Size(52, 28);
            this.tsbAutoCrop.Text = "On";
            this.tsbAutoCrop.ToolTipText = "Auto-crop label";
            this.tsbAutoCrop.Click += new System.EventHandler(this.tsbAutoCrop_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 31);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.Gray;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.pbPreview);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 31);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(584, 249);
            this.panel1.TabIndex = 2;
            // 
            // pbPreview
            // 
            this.pbPreview.BackColor = System.Drawing.Color.White;
            this.pbPreview.Location = new System.Drawing.Point(1, 1);
            this.pbPreview.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pbPreview.Name = "pbPreview";
            this.pbPreview.Size = new System.Drawing.Size(471, 198);
            this.pbPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbPreview.TabIndex = 1;
            this.pbPreview.TabStop = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCopyImage,
            this.tsmiSaveImage});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(197, 56);
            // 
            // tsmiCopyImage
            // 
            this.tsmiCopyImage.Name = "tsmiCopyImage";
            this.tsmiCopyImage.Size = new System.Drawing.Size(196, 26);
            this.tsmiCopyImage.Text = "Copy image";
            // 
            // tsmiSaveImage
            // 
            this.tsmiSaveImage.Image = ((System.Drawing.Image)(resources.GetObject("tsmiSaveImage.Image")));
            this.tsmiSaveImage.Name = "tsmiSaveImage";
            this.tsmiSaveImage.Size = new System.Drawing.Size(196, 26);
            this.tsmiSaveImage.Text = "Save image as...";
            // 
            // CommandPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "CommandPreview";
            this.Size = new System.Drawing.Size(584, 280);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
	}
}
