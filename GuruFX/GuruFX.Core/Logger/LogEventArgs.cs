using System;

namespace GuruFX.Core.Logger
{
	public class LogEventArgs : EventArgs
	{
		MessageType m_messageType = MessageType.Information;
		string m_message = string.Empty;

		public LogEventArgs(string message)
		{
			this.m_messageType = MessageType.Information;
			this.m_message = message;
		}

		public LogEventArgs(MessageType messageType, string message)
		{
			this.m_messageType = messageType;
			this.m_message = message;
		}

		public MessageType MessageType
		{
			get { return m_messageType; }
			set { m_messageType = value; }
		}

		public string Message
		{
			get { return m_message; }
			set { m_message = value; }
		}
	}
}
