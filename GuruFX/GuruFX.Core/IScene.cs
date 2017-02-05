using System;
using System.Collections.Concurrent;

namespace GuruFX.Core
{
	public interface IScene
	{
		ConcurrentDictionary<Guid, ISystem> Systems { get; set; }

		IEntity Root { get; }
		IEntity FindEntity(IEntity entity);
		IEntity FindEntity(Guid instanceID);
		IEntity RemoveEntity(IEntity entity);
		IEntity RemoveEntity(Guid instanceID);
	}
}