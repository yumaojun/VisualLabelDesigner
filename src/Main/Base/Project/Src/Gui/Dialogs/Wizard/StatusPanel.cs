// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version value="$version"/>
// </file>

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;

//using ICSharpCode.Core.Services;
using ICSharpCode.Core.AddIns;

//using ICSharpCode.Core.Properties;
using ICSharpCode.Core.AddIns.Codons;
using ICSharpCode.SharpDevelop.WinForms;

namespace ICSharpCode.SharpDevelop.Gui.Dialogs
{
	public class StatusPanel : UserControl
	{
		WizardDialog wizard;
		Bitmap backGround = null;
		
		Font smallFont;
		Font normalFont;
		Font boldFont; 
		
		public StatusPanel(WizardDialog wizard)
		{ 
			smallFont  = SD.ResourceService.LoadFont("SansSerif",  14, FontStyle.Regular, GraphicsUnit.World);
			normalFont = SD.ResourceService.LoadFont("Tahoma", 14, FontStyle.Regular, GraphicsUnit.World);
			boldFont   = SD.ResourceService.LoadFont("Tahoma", 14, FontStyle.Bold, GraphicsUnit.World);
			
			this.wizard = wizard;
			backGround = SD.ResourceService.GetBitmap("GeneralWizardBackground");
			Size = new Size(198, 400);
			ResizeRedraw  = false;
			
			//			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
		}
		
		protected override void OnPaintBackground(PaintEventArgs pe)
		{
			//    		base.OnPaintBackground(pe);
			if (backGround != null) {
				Graphics g = pe.Graphics;
				g.DrawImage(backGround, 0, 0, Width, Height);
			}
		}
		
		protected override void OnPaint(PaintEventArgs pe)
		{
			//    		base.OnPaint(pe);
			Graphics g = pe.Graphics;
			
			g.DrawString(SD.ResourceService.GetString("SharpDevelop.Gui.Dialogs.WizardDialog.StepsLabel"),
			             smallFont,
			             new SolidBrush(Color.Black),
			             10,
			             24 - smallFont.Height);
			
			g.DrawLine(new Pen(Color.Black), 10, 24, Width - 10, 24);
			
			int curNumber = 0;
			for (int i = 0; i < wizard.WizardPanels.Count; i = wizard.GetSuccessorNumber(i)) {
				Font curFont = wizard.ActivePanelNumber == i ? boldFont : normalFont;
				IDialogPanelDescriptor descriptor = ((IDialogPanelDescriptor)wizard.WizardPanels[i]);
				g.DrawString((1 + curNumber) + ". " + descriptor.Label, curFont, new SolidBrush(Color.Black), 10, 40 + curNumber * curFont.Height);
				++curNumber;
			}
		}
	}
}
