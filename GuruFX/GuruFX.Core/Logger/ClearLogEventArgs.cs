using System;

namespace GuruFX.Core.Logger
{
	public class ClearLogEventArgs : EventArgs
	{
		MessageType mMessageTypesToClear = MessageType.None;
		
		public ClearLogEventArgs(MessageType messageTypesToClear)
		{
			MessageTypesToClear = messageTypesToClear;
		}
		
		public MessageType MessageTypesToClear
		{
			get { return mMessageTypesToClear; }
			set { mMessageTypesToClear = value; }
		}
	}
}
