using System;

namespace GuruFX.Core.Logger
{
	public class ClearLogEventArgs : EventArgs
	{
		public ClearLogEventArgs(MessageType messageTypesToClear)
		{
			MessageTypesToClear = messageTypesToClear;
		}
		
		public MessageType MessageTypesToClear { get; set; }
	}
}
