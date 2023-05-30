using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiaSharp.SimpleText
{
	/// <summary>
	/// Text Alignment
	/// </summary>
	public enum STAlignment
	{
		AlignLeft    = 0x0001,
		AlignRight   = 0x0002,
		AlignHCenter = 0x0004, // horizontal center

		AlignTop     = 0x0020,
		AlignBottom  = 0x0040,
		AlignVCenter = 0x0080, // vertical center

		AlignCenter  = AlignVCenter | AlignHCenter
	}
}
