// Copyright (c) 2023 yumaojun@gmail.com & yumaojun@qq.com

using ICSharpCode.Core;
using ICSharpCode.SharpDevelop;
using ICSharpCode.SharpDevelop.Workbench;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.ZPLTextEditor
{
	public class ZPLTextViewContent : AbstractViewContent
	{
		ZPLTextEditorPanel editor;

		public override object Control
		{
			get
			{
				return editor;
			}
		}

		public ZPLTextViewContent(OpenedFile file) : base(file)
		{
			editor = new ZPLTextEditorPanel(this);
			editor.TextWasEdited += editor_TextWasEdited;
		}

		// 文件被修改
		void editor_TextWasEdited(object sender, EventArgs e)
		{
			PrimaryFile.MakeDirty();
		}

		/// <summary>
		/// 打开文件时被调用
		/// </summary>
		/// <param name="file"></param>
		/// <param name="stream"></param>
		public override void Load(OpenedFile file, Stream stream)
		{
			try
			{
				editor.ShowFile(new ZPLTextFile(stream));
			}
			catch (InvalidZPLTextException ex)
			{
				// call with a delay to work around a re-entrancy bug
				// when closing a workbench window while it is getting activated
				SD.MainThread.InvokeAsyncAndForget(delegate
				{
					MessageService.ShowHandledException(ex);
					if (WorkbenchWindow != null)
					{
						WorkbenchWindow.CloseWindow(true);
					}
				});
			}
		}

		/// <summary>
		/// 保存文件时被调用
		/// </summary>
		/// <param name="file"></param>
		/// <param name="stream"></param>
		public override void Save(OpenedFile file, Stream stream)
		{
			editor.Save(stream);
		}
	}
}
