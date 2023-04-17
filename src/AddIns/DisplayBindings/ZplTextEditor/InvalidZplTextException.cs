// Copyright (c) 2023 yumaojun@gmail.com & yumaojun@qq.com


using System;
using System.Runtime.Serialization;

namespace YProgramStudio.ZPLTextEditor
{
	[Serializable]
	public class InvalidZPLTextException : Exception
	{
		public InvalidZPLTextException() : base()
		{
		}
		
		public InvalidZPLTextException(string message) : base(message)
		{
		}
		
		public InvalidZPLTextException(string message, Exception innerException) : base(message, innerException)
		{
		}
		
		protected InvalidZPLTextException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
