using System;

namespace GuruFX.Core.Logger
{
	public class ClearLogEventArgs : EventArgs
	{
		MessageType m_messageTypesToClear = MessageType.None;
		
		public ClearLogEventArgs(MessageType messageTypesToClear)
		{
			this.MessageTypesToClear = messageTypesToClear;
		}
		
		public MessageType MessageTypesToClear
		{
			get { return m_messageTypesToClear; }
			set { m_messageTypesToClear = value; }
		}
	}
}
