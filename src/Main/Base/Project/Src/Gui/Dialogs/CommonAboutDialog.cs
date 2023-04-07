// Copyright (c) 2014 AlphaSierraPapa for the SharpDevelop Team
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using System.Drawing;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ICSharpCode.SharpDevelop.Gui.XmlForms;

namespace ICSharpCode.SharpDevelop.Gui
{
	public class ScrollBox : UserControl
	{
		string[] text;
		int[]    textHeights;
		
		Image  image;
		Timer  timer;
		int    scroll = -220;
		
		public int ScrollY {
			get {
				return scroll;
			}
			set {
				scroll = value;
			}
		}
		
		public Image Image {
			get {
				return image;
			}
			set {
				image = value;
			}
		}
		
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				timer.Stop();
				foreach (Control ctrl in Controls) {
					ctrl.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		public ScrollBox()
		{
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			Image = IconService.GetBitmap("Icons.AboutImage");
			
			Font = SD.WinForms.LoadFont("Tahoma", 10);
			text = new string[] {
				"\"允许您基于Zebra编程语言（ZPL）创建、预览和打印标签（Label），一个免费且易于使用的软件实用程序。\"\n    -- 余茂军 2023年3月3号",
				"\"为命令、参数、文本和注释提供具有不同颜色的全彩色ZPL代码编辑器。\n 您可以选择如何渲染ZPL代码：本地Zebra打印机或使用labely.com Web服务。\"",
				"\"当您选择使用“本地Zebra打印机”作为渲染器时，需要具有网络的工业ZPL打印机。在顶部菜单上输入打印机的IP地址，然后单击两个箭头进行连接。现在您可以使用打印和预览功能。\"",
				"\"提供文本、图形、条形码等主要ZPL命令的列表！ZPL命令列表可以按类别和名称排序。还可以使用搜索框按名称或描述查找。\"",
				"\"每个ZPL命令都有一个帮助面板说明，可以帮助您生成标签。\""
			};
			
			// randomize the order in which the texts are displayed
			Random rnd = new Random();
			for (int i = 0; i < text.Length; i++) {
				Swap(ref text[i], ref text[rnd.Next(i, text.Length)]);
			}
			
			timer = new Timer();
			timer.Interval = 30;
			timer.Tick += new EventHandler(ScrollDown);
			timer.Start();
		}
		
		void Swap(ref string a, ref string b)
		{
			string c = a;
			a = b;
			b = c;
		}
		
		void ScrollDown(object sender, EventArgs e)
		{
			++scroll;
			Refresh();
		}
		
		protected override void OnPaintBackground(PaintEventArgs pe)
		{
			if (image != null) {
				pe.Graphics.DrawImage(image, 0, 0, Width, Height);
			}
		}
		int curText = 0;
		protected override void OnPaint(PaintEventArgs pe)
		{
			Graphics g = pe.Graphics;
			if (textHeights == null) {
				textHeights = new int[text.Length];
				for (int i = 0; i < text.Length; ++i) {
					textHeights[i] = (int)g.MeasureString(text[i], Font, new SizeF(Width / 2, Height * 2)).Height;
				}
			}
			g.DrawString(text[curText],
			             Font,
			             Brushes.Black,
			             new Rectangle(Width / 2, 0 - scroll, Width / 2, Height * 2));
			
			if (scroll > textHeights[curText]) {
				curText = (curText + 1) % text.Length;
				scroll = -textHeights[curText] - Height;
			}
		}
	}
	
	#pragma warning disable 618
	public class CommonAboutDialog : XmlForm
	{
		public ScrollBox ScrollBox {
			get {
				return (ScrollBox)ControlDictionary["aboutPictureScrollBox"];
			}
		}
		
		public CommonAboutDialog()
		{
			SetupFromXmlStream(this.GetType().Assembly.GetManifestResourceStream("ICSharpCode.SharpDevelop.Resources.CommonAboutDialog.xfrm"));
			var aca = (AssemblyCopyrightAttribute)typeof(CommonAboutDialog).Assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0];
			ControlDictionary["copyrightLabel"].Text = "Copyright " + aca.Copyright;
		}
		
		protected override void SetupXmlLoader()
		{
			xmlLoader.StringValueFilter    = new SharpDevelopStringValueFilter();
			xmlLoader.PropertyValueCreator = new SharpDevelopPropertyValueCreator();
		}
	}
}
