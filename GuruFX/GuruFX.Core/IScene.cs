using System;
using System.Collections.Concurrent;

namespace GuruFX.Core
{
	public interface IScene : IBaseObject
	{
		ConcurrentDictionary<Guid, ISystem> Systems { get; set; }
		ConcurrentDictionary<Guid, IUpdateable> Updateables { get; set; }
	}
}