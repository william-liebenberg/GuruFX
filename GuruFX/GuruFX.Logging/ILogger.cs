using System;

namespace GuruFX.Core.Logger
{
	public interface ILogger
	{
		void Log(string msg);
		void Log(MessageType msgType, string msg);
		void Log(Exception ex, string msg);
		void Log(string format, params object[] items);
		void Clear();
		void Clear(MessageType msgType);
	}
}
