
namespace LabelsDesignerTest
{
    partial class MainForm
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.testControl1 = new LabelsDesignerTest.TestControl();
            this.SuspendLayout();
            // 
            // testControl1
            // 
            this.testControl1.Location = new System.Drawing.Point(12, 12);
            this.testControl1.Name = "testControl1";
            this.testControl1.Size = new System.Drawing.Size(500, 400);
			this.testControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.testControl1.TabIndex = 0;
            this.testControl1.Text = "testControl1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.testControl1);
            this.Name = "MainForm";
            this.Text = "Test";
            this.ResumeLayout(false);

        }

		#endregion

		private TestControl testControl1;
	}
}

