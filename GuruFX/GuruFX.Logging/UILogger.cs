using System;

namespace GuruFX.Core.Logger
{
	public class UILogger : ILogger
	{
		readonly object mLockSync = new object();

		public event EventHandler<ClearLogEventArgs> ClearMessages;

		public event EventHandler<LogEventArgs> MessageReceived;

		public void Log(string msg)
		{
			OnMessage(MessageType.Information, msg);
		}

		public void Log(MessageType t, string message)
		{
			OnMessage(t, message);
		}

		public void Log(Exception ex, string msg)
		{
			// TODO: Add the message as part of the exception error
			OnMessage(MessageType.Error, ex.ToString());
		}

		public void Log(string format, params object[] items)
		{
			Log(string.Format(format, items));
		}

		public void Log(MessageType t, string format, params object[] items)
		{
			Log(t, string.Format(format, items));
		}

		public void Clear()
		{
			Clear(MessageType.All);
		}

		public void Clear(MessageType flagLogsToClear)
		{
			lock (mLockSync)
			{
				ClearMessages?.Invoke(this, new ClearLogEventArgs(flagLogsToClear));
			}
		}

		protected virtual void OnMessage(MessageType t, string message)
		{
			lock (mLockSync)
			{
				MessageReceived?.Invoke(this, new LogEventArgs(t, message));
			}
		}
	}
}
