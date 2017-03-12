using System;
using System.IO;

namespace GuruFX.Core.Logger
{
	public class FileLogger : ILogger, IDisposable
	{
		private FileStream m_fileStream;
		private StreamWriter m_streamWriter;

		public string Filename { get; set; }

		public FileLogger(string filename)
		{
			Filename = filename;
			OpenStreams();
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
			m_streamWriter?.WriteLine(msg);
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

		private void OpenStreams()
		{
			FileInfo finf = new FileInfo(Path.GetFullPath(Filename));
			if (finf.Directory == null)
			{
				return;
			}

			if (!finf.Directory.Exists)
			{
				finf.Directory.Create();
			}

			try
			{
				m_fileStream = new FileStream(finf.FullName, FileMode.Create, FileAccess.Write, FileShare.Read);
				m_streamWriter = new StreamWriter(m_fileStream);
			}
			catch (Exception)
			{
				CloseStreams();
				throw;
			}
		}

		private void CloseStreams()
		{
			m_streamWriter?.Flush();
			m_fileStream?.Flush();

			m_streamWriter?.Dispose();
			m_fileStream?.Dispose();

			m_streamWriter = null;
			m_fileStream = null;
		}

		#region IDisposable Support
		private bool m_disposedValue; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!m_disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
					CloseStreams();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				m_disposedValue = true;
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
