// Copyright (c) 2023 yumaojun@gmail.com & yumaojun@qq.com

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using FastColoredTextBoxNS;
using ICSharpCode.SharpDevelop;
using ICSharpCode.SharpDevelop.Workbench;
using YProgramStudio.ZPLTextEditor.Gui;
using YProgramStudio.ZPLTextEditor.Properties;
using YProgramStudio.ZPLTextEditor.Services;

namespace YProgramStudio.ZPLTextEditor
{
	public partial class ZPLTextEditorPanel : UserControl
	{
		private List<ZPLCommand> lzplCode = new List<ZPLCommand>();

		private ZPLTextFile activeZplTextFile;

		public FastColoredTextBox ZplCodeTextBox { get; private set; }

		public event EventHandler TextWasEdited;

		public event EventHandler DocumentChange;

		public event EventHandler ZplCommandHelp;

		public Style CommentStyle { get; set; }

		public Style FunctionStyle { get; set; }

		public Style FunctionOptionsStyle { get; set; }

		public Style StringStyle { get; set; }

		public Style VariableStyle { get; set; }

		public bool ReplaceSpecialChar { get; set; }

		public string DocumentFullName { get; set; }

		private string _documentName;
		public string DocumentName
		{
			get
			{
				return this._documentName;
			}
			set
			{
				this._documentName = value;
				this.HasNotSaveChange = false;
			}
		}

		private bool _hasNotSaveChange;
		public bool HasNotSaveChange
		{
			get
			{
				return this._hasNotSaveChange;
			}
			set
			{
				this._hasNotSaveChange = value;
				this.OnDocumentChange(new EventArgs());
			}
		}

		private void fctbZplCode_ReplaceSpecialChar(object sender, KeyEventArgs e)
		{
			if (this.ZplCodeTextBox.SelectionStart > 0)
			{
				string a = this.ZplCodeTextBox.Text.Substring(this.ZplCodeTextBox.SelectionStart - 1, 1);
				if (a == "é")
				{
					this.ZplCodeTextBox.SelectionStart = this.ZplCodeTextBox.SelectionStart - 1;
					this.ZplCodeTextBox.SelectionLength = 1;
					this.ZplCodeTextBox.SelectedText = "_84";
				}
			}
		}

		protected virtual void OnDocumentChange(EventArgs e)
		{
			if (this.DocumentChange != null)
			{
				this.DocumentChange(this, e);
			}
		}

		protected virtual void OnZplCommandHelp(object sender, EventArgs e)
		{
			if (this.ZplCommandHelp != null)
			{
				this.ZplCommandHelp(sender, e);
			}
		}

		ZPLTextViewContent viewContent;

		bool IsActiveViewContent
		{
			get { return SD.Workbench.ActiveViewContent == viewContent; }
		}

		private ZPLTextEditorPanel()
		{
			InitializeComponent();
			InitialCustomized();
		}

		public ZPLTextEditorPanel(ZPLTextViewContent vc) : this()
		{
			viewContent = vc;
		}

		public void InitialCustomized()
		{
			this.ZplCodeTextBox.Font = new Font(Settings.Default.EditorFontFamily, float.Parse(Settings.Default.EditorFontSize));
			this.ZplCodeTextBox.ForeColor = Settings.Default.RegularStyleColor;
			this.CommentStyle = new TextStyle(new SolidBrush(Settings.Default.CommentStyleColor), null, FontStyle.Italic);
			this.FunctionStyle = new TextStyle(new SolidBrush(Settings.Default.FunctionStyleColor), null, FontStyle.Regular);
			this.FunctionOptionsStyle = new TextStyle(new SolidBrush(Settings.Default.OptionsStyleColor), null, FontStyle.Regular);
			this.StringStyle = new TextStyle(new SolidBrush(Settings.Default.StringStyleColor), null, FontStyle.Regular);
			this.VariableStyle = new TextStyle(new SolidBrush(Settings.Default.VariableStyleColor), null, FontStyle.Regular);
			this.ReplaceSpecialChar = Settings.Default.ReplaceSpecialChar;
			if (CommandHelper.Instance != null)
			{
				CommandHelper.Instance.BtnOkClick += Command_BtnOkClick;
			}
		}

		public void Command_BtnOkClick(object sender, EventArgsString e)
		{
			if (IsActiveViewContent)
			{
				this.ZplCodeTextBox.SelectedText = e.MyEventString;
				this.ZplCodeTextBox.Focus();
			}
		}

		private void CodeEditor_Load(object sender, EventArgs e)
		{
			if (this.ReplaceSpecialChar)
			{
				this.ZplCodeTextBox.KeyUp += this.fctbZplCode_ReplaceSpecialChar;
			}
			this.ZplCodeTextBox.ShowScrollBars = true;
			this.lzplCode = ZPLCommand.DeserializeFromXML();
		}

		private void UpdateHelp()
		{
			if (this.ZplCodeTextBox.SelectionStart > 0)
			{
				int num = 1;
				while (num < 40 && this.ZplCodeTextBox.SelectionStart - num != -1)
				{
					try
					{
						if (this.ZplCodeTextBox.Text.Substring(this.ZplCodeTextBox.SelectionStart - num, 1) == "^")
						{
							string strZplCommand = this.ZplCodeTextBox.Text.Substring(this.ZplCodeTextBox.SelectionStart - num, 3);
							if (strZplCommand.Substring(0, 2) == "^A" && strZplCommand != "^A@")
							{
								strZplCommand = "^A";
							}
							ZPLCommand tag = (from x in this.lzplCode
											  where x.Name == strZplCommand.ToUpper()
											  select x).First<ZPLCommand>();
							this.OnZplCommandHelp(new ToolStripButton
							{
								Tag = tag
							}, EventArgs.Empty);
							break;
						}
					}
					catch (Exception)
					{
					}
					num++;
				}
			}
		}

		private void fctbZplCode_KeyUp(object sender, KeyEventArgs e)
		{
			this.UpdateHelp();
		}

		private void fctbZplCode_MouseUp(object sender, MouseEventArgs e)
		{
			this.UpdateHelp();
		}

		private void fctbZplCode_TextChanged(object sender, TextChangedEventArgs e)
		{
			this.HasNotSaveChange = true;
			e.ChangedRange.ClearStyle(new Style[]
			{
				this.CommentStyle
			});
			e.ChangedRange.ClearStyle(new Style[]
			{
				this.VariableStyle
			});
			e.ChangedRange.ClearStyle(new Style[]
			{
				this.FunctionStyle
			});
			e.ChangedRange.ClearStyle(new Style[]
			{
				this.StringStyle
			});
			e.ChangedRange.ClearStyle(new Style[]
			{
				this.FunctionOptionsStyle
			});
			e.ChangedRange.SetStyle(this.FunctionStyle, "\\^A[0-Z]|^A@", RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(this.FunctionStyle, "\\^B[0|1|2|3|4|5|7|8|9|A|B|C|D|E|F|I|J|K|L|M|O|P|Q|R|S|T|U|X|Y|Z]", RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(this.FunctionStyle, "\\^C[C|D|F|I|M|O|T|V|W]", RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(this.FunctionStyle, "\\~C[C|T]", RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(this.FunctionStyle, "\\^D[F|N]", RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(this.FunctionStyle, "\\~D[B|E|G|N|S|T|U|Y]", RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(this.FunctionStyle, "\\^F[B|C|D|H|L|M|N|O|P|R|S|T|V|W]", RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(this.FunctionStyle, "\\^F[B|C|D|H|L|M|N|O|P|R|S|T|V|W|8|16]", RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(this.FunctionStyle, "\\^G[B|C|D|E|F|S]", RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(this.FunctionStyle, "\\^H[F|G|H|T|V|W|Y|Z]", RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(this.FunctionStyle, "\\~H[B|D|I|M|Q|S|U|Y|Z]", RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(this.FunctionStyle, "\\^I[D|L|M|S]", RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(this.FunctionStyle, "\\^J[B|H|I|J|M|S|W]", RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(this.FunctionStyle, "\\~J[A|B|C|D|E|F|G|I|L|N|O|P|Q|R|S|X]", RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(this.FunctionStyle, "\\^K[D|L|N|P|M|S|W]", RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(this.FunctionStyle, "\\~KB", RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(this.FunctionStyle, "\\^L[F|H|L|R|S|T]", RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(this.FunctionStyle, "\\^M[A|C|D|F|I|L|M|N|P|T|U|W]", RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(this.FunctionStyle, "\\^N[C|D|I|S]", RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(this.FunctionStyle, "\\~N[C|R|T]", RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(this.FunctionStyle, "\\^P[A|F|H|M|O|P|Q|R|W]", RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(this.FunctionStyle, "\\~P[H|P|R|S]", RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(this.FunctionStyle, "\\^RO", RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(this.FunctionStyle, "\\^S[C|E|F|I|L|N|O|P|Q|R|S|T|X|Z]", RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(this.FunctionStyle, "\\~SD", RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(this.FunctionStyle, "\\^T[B|O]", RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(this.FunctionStyle, "\\~TA", RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(this.FunctionStyle, "\\^WD", RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(this.FunctionStyle, "\\~W[C|Q]", RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(this.FunctionStyle, "\\^X[A|B|F|G|S|Z]", RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(this.FunctionStyle, "\\^ZZ", RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(this.FunctionStyle, "\\,");
			e.ChangedRange.SetStyle(this.VariableStyle, string.Format("{0}.*?{1}", Regex.Escape("${"), Regex.Escape("}$")), RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(this.StringStyle, "\\^FD([^^]*)\\^FS", RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(this.StringStyle, "\\^FV([^^]*)\\^FS", RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(this.FunctionOptionsStyle, "(\\,|\\#|[0-9])([0-Z^\\,|.|\\\"]{1,100})[\\r|`,|\\^]*", RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(this.FunctionOptionsStyle, "\\^[0-Z][0-Z]([0-Z\\,]{1,100})[\\,|\\r|\\^\\s]", RegexOptions.IgnoreCase);
			e.ChangedRange.SetStyle(this.CommentStyle, "(\\^FX.+?(?=\\^|$))", RegexOptions.Multiline);
			this.UpdateHelp();

			// TODO: 需要关注是否存在BUG，即首次打开文件也是待保存
			if (this.TextWasEdited != null)
			{
				this.TextWasEdited(this, e);
			}
		}

		/// <summary>
		/// save
		/// </summary>
		/// <param name="stream"></param>
		public void Save(Stream stream)
		{
			this.activeZplTextFile.Text = this.ZplCodeTextBox.Text;
			this.activeZplTextFile.Save(stream);
		}

		/// <summary>
		/// Show by new or open
		/// </summary>
		/// <param name="zplTextFile"></param>
		public void ShowFile(ZPLTextFile zplTextFile)
		{
			this.activeZplTextFile = zplTextFile;
			this.ZplCodeTextBox.Clear();
			this.ZplCodeTextBox.SelectedText = this.activeZplTextFile.Text;
			if (this.ZplCodeTextBox.LinesCount > 1)
			{
				this.ZplCodeTextBox.Selection.Start = new Place(0, 1);
			}
			this.ZplCodeTextBox.Focus();
		}
	}
}
