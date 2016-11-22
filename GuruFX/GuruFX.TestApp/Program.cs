using GuruFX.Core.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuruFX.TestApp
{
	class Program
	{
		static void Main(string[] args)
		{
			Logger.Instance.MessageReceived += Instance_MessageReceived;
			Logger.Instance.Log("We have a logger!");
			Logger.Instance.Log(MessageType.CompilerError, "This is a compiler error!");
			Logger.Instance.Log(MessageType.Error, "This is a compiler error!");
			Logger.Instance.Log(MessageType.Information, "This is Information!");
			Logger.Instance.Log(MessageType.None, "This is a nothing!");
			Logger.Instance.Log(MessageType.RuntimeStatistics, "This is a Runtime Statistic!");
			Logger.Instance.Log(MessageType.Warning, "This is a Warning!");
			Logger.Instance.Log(new Exception("This is an exception!"));

			Logger.Instance.Log("This is just a test");
		}

		private static void Instance_MessageReceived(object sender, LogEventArgs e)
		{
			Console.WriteLine(e.MessageType.ToString() + ": " + e.Message);
		}
	}
}
