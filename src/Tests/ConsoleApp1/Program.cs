using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
	/**
 * Encoding mode.
 */
	enum QRencodeMode
	{
		QR_MODE_NUL = -1,   ///< Terminator (NUL character). Internal use only
		QR_MODE_NUM = 0,    ///< Numeric mode
		QR_MODE_AN,         ///< Alphabet-numeric mode
		QR_MODE_8,          ///< 8-bit data mode
		QR_MODE_KANJI,      ///< Kanji (shift-jis) mode
		QR_MODE_STRUCTURE,  ///< Internal use only
		QR_MODE_ECI,        ///< ECI mode
		QR_MODE_FNC1FIRST,  ///< FNC1, first position
		QR_MODE_FNC1SECOND, ///< FNC1, second position
	}
   ;

	/**
	 * Level of error correction.
	 */
	enum QRecLevel
	{
		QR_ECLEVEL_L = 0, ///< lowest
		QR_ECLEVEL_M,
		QR_ECLEVEL_Q,
		QR_ECLEVEL_H      ///< highest
	}
   ;

	struct Barcode
	{
		public int Version { get; set; }
		public int Width { get; set; }
		public int[] Data { get; set; }
	}

	class Program
	{
		[DllImport("qrencode.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern UInt32 QRcode_encodeString(string str1, int int1, QRecLevel l, QRencodeMode m, int int2);

		//public Barcode Bar
		//{
		//	get => QRcode_encodeString("1", 0, QRecLevel.QR_ECLEVEL_M, QRencodeMode.QR_MODE_8, 1);
		//}

		static void Main(string[] args)
		{
			//var objPtr = QRcode_encodeString("1", 0, QRecLevel.QR_ECLEVEL_M, QRencodeMode.QR_MODE_8, 1);

			//int objPtrCount = Marshal.ReadInt32(objPtr);
			//for (int i = 0; i < objPtrCount; i++)
			//{
			//	IntPtr groupPtr1 = Marshal.ReadIntPtr(objPtr, 4);
			//	var v1 = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr(groupPtr1));

			//}
			ConsoleDriver.Run(SampleDriver);
		}
	}
}
