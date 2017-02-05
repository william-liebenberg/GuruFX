using System;
using System.Collections.Generic;

namespace GuruFX.Core.Logger
{
	public class ConsoleLogger : ILogger
	{
		readonly Dictionary<MessageType, ConsoleColor> mConsoleColors = new Dictionary<MessageType, ConsoleColor>
		{
			{ MessageType.None, ConsoleColor.Magenta },
			{ MessageType.Information, ConsoleColor.White },
			{ MessageType.Warning, ConsoleColor.Yellow },
			{ MessageType.Error, ConsoleColor.Red },
			{ MessageType.CompilerError, ConsoleColor.Cyan },
			{ MessageType.RuntimeStatistics, ConsoleColor.Green }
		};

		public void Clear()
		{
			Console.Clear();
		}

		public void Clear(MessageType msgType)
		{
			// can't clear anything specific, just the whole console.
			Console.Clear();
		}

		public void Log(string msg)
		{
			Console.WriteLine(msg);
		}

		public void Log(string format, params object[] items)
		{
			Console.ForegroundColor = mConsoleColors[MessageType.Information];
			Log(MessageType.Information, string.Format(format, items));
			Console.ResetColor();
		}

		public void Log(Exception ex, string msg)
		{
			Console.ForegroundColor = mConsoleColors[MessageType.Error];
			Log(MessageType.Error, msg + Environment.NewLine + Environment.NewLine + "Exception Details: " + ex);
			Console.ResetColor();
		}

		public void Log(MessageType msgType, string msg)
		{
			Console.ForegroundColor = mConsoleColors[msgType];
			Log(msgType.ToString() + ": " + msg);
			Console.ResetColor();
		}
	}
}
