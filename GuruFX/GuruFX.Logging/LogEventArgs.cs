using System;

namespace GuruFX.Core.Logger
{
	public class LogEventArgs : EventArgs
	{
		MessageType mMessageType = MessageType.Information;
		string mMessage = string.Empty;

		public LogEventArgs(string message)
		{
			mMessageType = MessageType.Information;
			mMessage = message;
		}

		public LogEventArgs(MessageType messageType, string message)
		{
			mMessageType = messageType;
			mMessage = message;
		}

		public MessageType MessageType
		{
			get { return mMessageType; }
			set { mMessageType = value; }
		}

		public string Message
		{
			get { return mMessage; }
			set { mMessage = value; }
		}
	}
}
