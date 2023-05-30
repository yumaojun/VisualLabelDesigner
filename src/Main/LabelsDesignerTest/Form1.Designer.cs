
namespace LabelsDesignerTest
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.origin1 = new YProgramStudio.LabelsDesigner.Gui.Origin();
            this.panel1 = new System.Windows.Forms.Panel();

			this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // origin1
            // 
            this.origin1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.origin1.Location = new System.Drawing.Point(0, 0);
            this.origin1.Margin = new System.Windows.Forms.Padding(0);
            this.origin1.Name = "origin1";
            this.origin1.Size = new System.Drawing.Size(23, 23);
            this.origin1.TabIndex = 0;
            this.origin1.Text = "origin1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.origin1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 450);
            this.panel1.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private YProgramStudio.LabelsDesigner.Gui.Origin origin1;
		private System.Windows.Forms.Panel panel1;
	}
}