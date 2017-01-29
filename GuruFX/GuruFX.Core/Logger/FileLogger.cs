using System;
using System.IO;

namespace GuruFX.Core.Logger
{
	public class FileLogger : ILogger, IDisposable
	{
		private System.IO.FileStream mFileStream;
		private System.IO.StreamWriter mStreamWriter;

		public string Filename { get; set; }

		public FileLogger(string filename)
		{
			this.Filename = filename;
			FileLogger.OpenStreams(this);
		}

		public void Clear()
		{
			// hmmm delete and reopen the file??
		}

		public void Clear(MessageType msgType)
		{
			// hmmm delete and reopen the file??
		}

		public void Log(string msg)
		{
			mStreamWriter?.WriteLine(msg);
		}

		public void Log(string format, params object[] items)
		{
			Log(MessageType.Information, string.Format(format, items));
		}

		public void Log(Exception ex, string msg)
		{
			Log(MessageType.Error, msg + Environment.NewLine + Environment.NewLine + "Exception Details: " + ex);
		}

		public void Log(MessageType msgType, string msg)
		{
			Log(msgType.ToString() + ": " + msg);
		}

		private static void OpenStreams(FileLogger instance)
		{
			var finf = new FileInfo(Path.GetFullPath(instance.Filename));
			if (!finf.Directory.Exists)
			{
				finf.Directory.Create();
			}

			try
			{
				instance.mFileStream = new FileStream(finf.FullName, FileMode.Create, FileAccess.Write, FileShare.Read);
				instance.mStreamWriter = new StreamWriter(instance.mFileStream);
			}
			catch (Exception ex)
			{
				CloseStreams(instance);
				throw ex;
			}
		}

		private static void CloseStreams(FileLogger instance)
		{
			instance.mStreamWriter?.Flush();
			instance.mFileStream?.Flush();

			instance.mStreamWriter?.Close();
			instance.mFileStream?.Close();

			instance.mStreamWriter = null;
			instance.mFileStream = null;
		}

		#region IDisposable Support
		private bool disposedValue; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
					FileLogger.CloseStreams(this);
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~FileLogger() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}
		#endregion
	}
}
