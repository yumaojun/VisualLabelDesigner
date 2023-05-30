using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YProgramStudio.LabelsDesigner.Model;

namespace YProgramStudio.LabelsDesigner.Labels
{
	/// <summary>
	/// 模板路径绘制
	/// </summary>
	public class RollTemplatePath : SkiaSharp.SKPath
	{
		public RollTemplatePath(Template template)
		{
			Distance x0 = (template.PageWidth - template.RollWidth) / 2.0f;
			Distance w = template.RollWidth;
			Distance h = template.PageHeight;
			Distance c = 0.07f * h;
			Distance b = 0.03f * h;

			const int nx = 18;

			// Upper break line
			MoveTo(x0.Pt(), -c.Pt());
			for (int ix = 1; ix <= nx; ix++)
			{
				Distance x = ix * (w / (float)(nx)) + x0;
				float a = (float)(ix * (2f * Math.PI / (float)(nx)));

				LineTo(x.Pt(), b.Pt() * (float)Math.Sin(a) - c.Pt());
			}

			// Lower break line
			LineTo((x0 + w).Pt(), (h + c).Pt());
			for (int ix = nx - 1; ix >= 0; ix--)
			{
				Distance x = ix * (w / (float)(nx)) + x0;
				float a = (float)(ix * (2f * Math.PI / (float)(nx)));

				LineTo(x.Pt(), b.Pt() * (float)Math.Sin(a) + h.Pt() + c.Pt());
			}

			// Close path
			Close();
		}
	}
}
