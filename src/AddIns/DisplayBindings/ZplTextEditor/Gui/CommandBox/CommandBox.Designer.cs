
using System.Collections.Generic;

namespace YProgramStudio.ZPLTextEditor.Gui
{
	partial class CommandBox
	{
		/// <summary> 
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		private List<Services.ZPLCommand> li_ZplCode = new List<Services.ZPLCommand>();

		private const string cSearchText = "Search ...";

		private System.Windows.Forms.ToolStrip tsTool;

		private System.Windows.Forms.SplitContainer splitContainer1;

		private DelayedTextBox tbSearch;

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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tbSearch = new YProgramStudio.ZPLTextEditor.Gui.DelayedTextBox();
            this.tsTool = new System.Windows.Forms.ToolStrip();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tbSearch);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.Controls.Add(this.tsTool);
            this.splitContainer1.Size = new System.Drawing.Size(144, 296);
            this.splitContainer1.SplitterDistance = 28;
            this.splitContainer1.TabIndex = 121;
            // 
            // tbSearch
            // 
            this.tbSearch.DelayedTextChangedTimeout = 200;
            this.tbSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSearch.Location = new System.Drawing.Point(0, 0);
            this.tbSearch.MaxLength = 100;
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Size = new System.Drawing.Size(144, 25);
            this.tbSearch.TabIndex = 60;
            this.tbSearch.TabStop = false;
            this.tbSearch.TextChanged += new System.EventHandler(this.TstbSearch_TextChanged);
            this.tbSearch.Enter += new System.EventHandler(this.TstbSearch_GotFocus);
            this.tbSearch.Leave += new System.EventHandler(this.TstbSearch_LostFocus);
            // 
            // tsTool
            // 
            this.tsTool.AllowMerge = false;
            this.tsTool.BackColor = System.Drawing.SystemColors.Control;
            this.tsTool.CanOverflow = false;
            this.tsTool.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsTool.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.tsTool.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.tsTool.Location = new System.Drawing.Point(0, 0);
            this.tsTool.Name = "tsTool";
            this.tsTool.Padding = new System.Windows.Forms.Padding(0);
            this.tsTool.ShowItemToolTips = false;
            this.tsTool.Size = new System.Drawing.Size(144, 102);
            this.tsTool.Stretch = true;
            this.tsTool.TabIndex = 0;
            this.tsTool.Text = "toolStrip1";
            // 
            // CommandBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "CommandBox";
            this.Size = new System.Drawing.Size(144, 296);
            this.Load += new System.EventHandler(this.zToolbox_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion
	}
}
