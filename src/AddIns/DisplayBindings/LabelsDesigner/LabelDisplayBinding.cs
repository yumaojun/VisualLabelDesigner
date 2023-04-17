using ICSharpCode.SharpDevelop.Workbench;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner
{
	public class LabelDisplayBinding : IDisplayBinding
	{
		public double AutoDetectFileContent(ICSharpCode.Core.FileName fileName, Stream fileContent, string detectedMimeType)
		{
			throw new NotImplementedException();
		}

		public bool CanCreateContentForFile(ICSharpCode.Core.FileName fileName)
		{
			return true;
		}

		public IViewContent CreateContentForFile(OpenedFile file)
		{
			return new LabelViewContent(file);
		}

		public bool IsPreferredBindingForFile(ICSharpCode.Core.FileName fileName)
		{
			return true;
		}
	}
}
