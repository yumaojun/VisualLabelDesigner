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

namespace VisualLabelDesigner.ZplTextEditor
{
	public class ZplTextViewContent : AbstractViewContent
	{
		ZplTextEditorPanel editor;

		public override object Control
		{
			get
			{
				return editor;
			}
		}

		public ZplTextViewContent(OpenedFile file) : base(file)
		{
			editor = new ZplTextEditorPanel(this);
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
				editor.ShowFile(new ZplTextFile(stream));
			}
			catch (InvalidZplTextException ex)
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
