using System;
using System.Collections.Concurrent;
using GuruFX.Core.Entities;

namespace GuruFX.Core.Scenes
{
	public class Scene : IScene
	{
		public ConcurrentDictionary<Guid, ISystem> Systems { get; set; } = new ConcurrentDictionary<Guid, ISystem>();

		public IEntity Root { get; } = new Entity("Scene Root");

		public IEntity FindEntity(IEntity entity) => Root.FindEntity(entity, true);

		public IEntity FindEntity(Guid instanceID) => Root.FindEntity(instanceID, true);

		public IEntity RemoveEntity(IEntity entity) => Root.RemoveEntity(entity);

		public IEntity RemoveEntity(Guid instanceID) => Root.RemoveEntity(instanceID);
	}
}
