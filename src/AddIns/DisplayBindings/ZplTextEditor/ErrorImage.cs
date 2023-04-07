using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualLabelDesigner.ZplTextEditor
{
	public class ErrorImage
	{
		public static Image Generate(string errorMessage)
		{
			PointF point = new PointF(78f, 38f);
			Bitmap bitmap = (Bitmap)Image.FromFile("include\\no_label.png");
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				using (Font font = new Font("Consolas", 8f))
				{
					graphics.DrawString(errorMessage, font, Brushes.Black, point);
				}
			}
			return bitmap;
		}
	}
}
