// Copyright (c) 2023 yumaojun@gmail.com & yumaojun@qq.com


using System;
using System.Runtime.Serialization;

namespace VisualLabelDesigner.ZplTextEditor
{
	[Serializable]
	public class InvalidZplTextException : Exception
	{
		public InvalidZplTextException() : base()
		{
		}
		
		public InvalidZplTextException(string message) : base(message)
		{
		}
		
		public InvalidZplTextException(string message, Exception innerException) : base(message, innerException)
		{
		}
		
		protected InvalidZplTextException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
