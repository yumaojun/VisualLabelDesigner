// Copyright (c) 2023 yumaojun@gmail.com & yumaojun@qq.com

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.ZPLTextEditor
{
	public sealed class ZPLTextFile
	{
		public string Text { get; set; } = "^XA\n\n^XZ";

		public ZPLTextFile() { }

		public ZPLTextFile(Stream stream)
		{
			if (stream == null)
				throw new ArgumentNullException("stream");
			if (!stream.CanRead)
				throw new ArgumentException("The stream must be readable", "stream");
			if (!stream.CanSeek)
				throw new ArgumentException("The stream must be seekable", "stream");

			LoadText(stream);
		}

		void LoadText(Stream stream)
		{
			StreamReader r = new StreamReader(stream);
			Text = r.ReadToEnd();
		}

		public void Save(string fileName)
		{
			if (fileName == null)
				throw new ArgumentNullException("fileName");
			using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
			{
				Save(fs);
			}
		}

		public void Save(Stream stream)
		{
			if (stream == null)
				throw new ArgumentNullException("stream");
			if (!stream.CanWrite)
				throw new ArgumentException("The stream must be writeable", "stream");
			if (!stream.CanSeek)
				throw new ArgumentException("The stream must be seekable", "stream");

			if (Text != null)
			{
				byte[] entryData = Encoding.UTF8.GetBytes(Text);
				stream.Write(entryData, 0, entryData.Length);
			}
		}
	}
}
