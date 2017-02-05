using System;

namespace GuruFX.Core.Logger
{
	[Flags]
	public enum MessageType
	{
		None = 0,
		Information = 1,
		RuntimeStatistics = 2,
		Warning = 3,
		Error = 4,
		CompilerError = 5,
		All = 6
	};
}
