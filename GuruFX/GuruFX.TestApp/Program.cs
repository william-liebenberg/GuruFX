using System;
using GuruFX.Core.Logger;

namespace GuruFX.TestApp
{
	class Program
	{
		static void Main(string[] args)
		{
			//using (var logger = new FileLogger("GuruFX.log"))
			var logger = new ConsoleLogger();
			{
				logger.Log("Message");
				logger.Log(MessageType.Information, "This is Information");
				logger.Log(MessageType.Warning, "This is a Warning");
				logger.Log(MessageType.Error, "This is an Error");
				logger.Log(MessageType.CompilerError, "This is Compiler Error");
				logger.Log(MessageType.RuntimeStatistics, "This is a Runtime Stat");
				logger.Log(new Exception("ExceptionMessage"), "I made an exception out of thin air!");
			}
		}
	}
}
