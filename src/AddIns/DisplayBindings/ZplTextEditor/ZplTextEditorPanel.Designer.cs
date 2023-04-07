
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VisualLabelDesigner.ZplTextEditor
{
	partial class ZplTextEditorPanel
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
			components = new System.ComponentModel.Container();
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ZplCodeTextBox = new FastColoredTextBoxNS.FastColoredTextBox();
			((ISupportInitialize)this.ZplCodeTextBox).BeginInit();
			base.SuspendLayout();
			this.ZplCodeTextBox.AllowDrop = false;
			this.ZplCodeTextBox.AllowMacroRecording = false;
			this.ZplCodeTextBox.AutoCompleteBracketsList = new char[]
			{
				'(',
				')',
				'{',
				'}',
				'[',
				']',
				'"',
				'"',
				'\'',
				'\''
			};
			this.ZplCodeTextBox.AutoScrollMinSize = new Size(0, 15);
			this.ZplCodeTextBox.BackBrush = null;
			this.ZplCodeTextBox.CharHeight = 15;
			this.ZplCodeTextBox.CharWidth = 7;
			this.ZplCodeTextBox.CommentPrefix = "^FX";
			this.ZplCodeTextBox.CurrentLineColor = Color.Gray;
			this.ZplCodeTextBox.Cursor = Cursors.IBeam;
			this.ZplCodeTextBox.DisabledColor = Color.FromArgb(100, 180, 180, 180);
			this.ZplCodeTextBox.Dock = DockStyle.Fill;
			this.ZplCodeTextBox.Font = new Font("Consolas", 9.75f);
			this.ZplCodeTextBox.ImeMode = ImeMode.On;
			this.ZplCodeTextBox.IsReplaceMode = false;
			this.ZplCodeTextBox.Location = new Point(0, 0);
			this.ZplCodeTextBox.Name = "fctbZplCode";
			this.ZplCodeTextBox.Paddings = new Padding(0);
			this.ZplCodeTextBox.SelectionColor = Color.FromArgb(60, 0, 0, 255);
			this.ZplCodeTextBox.Size = new Size(325, 181);
			this.ZplCodeTextBox.TabIndex = 0;
			this.ZplCodeTextBox.WordWrap = true;
			this.ZplCodeTextBox.Zoom = 100;
			this.ZplCodeTextBox.TextChanged += this.fctbZplCode_TextChanged;
			this.ZplCodeTextBox.KeyDown += this.fctbZplCode_KeyDown;
			this.ZplCodeTextBox.KeyUp += this.fctbZplCode_KeyUp;
			this.ZplCodeTextBox.MouseUp += this.fctbZplCode_MouseUp;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.ZplCodeTextBox);
			base.Name = "zplTextEditor";
			base.Size = new Size(325, 181);
			base.Load += this.CodeEditor_Load;
			((ISupportInitialize)this.ZplCodeTextBox).EndInit();
			base.ResumeLayout(false);
		}

		private void fctbZplCode_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.OemCloseBrackets)
			{
				SendKeys.Send("B");
				SendKeys.Send("{BS}");
			}
		}


		#endregion
	}
}
