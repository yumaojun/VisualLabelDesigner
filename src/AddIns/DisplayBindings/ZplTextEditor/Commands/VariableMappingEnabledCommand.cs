using ICSharpCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.ZPLTextEditor
{
	public class VariableMappingEnabledCommand : AbstractCheckableMenuCommand
	{
		public override void Run()
		{
			base.Run();
			var t = this.Owner.GetType();
			var str = t.FullName;
			System.Diagnostics.Debug.WriteLine(str); // WpfWorkbench
		}
	}
}
