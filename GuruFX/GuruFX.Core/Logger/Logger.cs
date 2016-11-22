using System;

namespace GuruFX.Core.Logger
{
	public class Logger : Singleton<Logger>
	{
		readonly object mLockSync = new object();

		public event EventHandler<ClearLogEventArgs> ClearMessages;

		public event EventHandler<LogEventArgs> MessageReceived;

		public void Log(string msg)
		{
			this.OnMessage(MessageType.Information, msg);
		}

		public void Log(MessageType t, string message)
		{
			this.OnMessage(t, message);
		}

		public void Log(Exception ex)
		{
			this.OnMessage(MessageType.Error, ex.ToString());
		}

		public void Log(string format, params object[] items)
		{
			this.Log(string.Format(format, items));
		}

		public void Log(MessageType t, string format, params object[] items)
		{
			this.Log(t, string.Format(format, items));
		}

		public void ClearLog()
		{
			this.ClearLog(MessageType.All);
		}

		public void ClearLog(MessageType flagLogsToClear)
		{
			lock (mLockSync)
			{
				this.ClearMessages?.Invoke(this, new ClearLogEventArgs(flagLogsToClear));
			}
		}

		protected virtual void OnMessage(MessageType t, string message)
		{
			lock (mLockSync)
			{
				this.MessageReceived?.Invoke(this, new LogEventArgs(t, message));
			}
		}
	}
}
