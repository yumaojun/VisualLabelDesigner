// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version value="$version"/>
// </file>

using System;
using System.Collections;
using System.CodeDom.Compiler;
using System.Windows.Forms;

using ICSharpCode.Core.AddIns;
using ICSharpCode.Core.AddIns.Codons;
using System.Collections.Generic;
//using ICSharpCode.Core.Properties;

namespace ICSharpCode.Core.AddIns.Codons
{
	public interface IDialogPanelDescriptor
	{
		/// <value>
		/// Returns the ID of the dialog panel codon
		/// </value>
		string ID {
			get;
		}
		
		/// <value>
		/// Returns the label of the dialog panel
		/// </value>
		string Label {
			get;
			set;
		}

		List<IDialogPanelDescriptor> DialogPanelDescriptors {
			get;
			set;
		}
		
		/// <value>
		/// Returns the dialog panel object
		/// </value>
		IDialogPanel DialogPanel {
			get;
			set;
		}
	}
}
