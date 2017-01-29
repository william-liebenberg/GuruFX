using System;
using System.Collections.Generic;
using GuruFX.Core.Entities;

namespace GuruFX.Core.Scenes
{
	public class Scene : IScene
	{
		readonly Dictionary<Guid, IEntity> mEntities = new Dictionary<Guid, IEntity>();

		public IEntity CreateEntity()
		{
			var entity = new Entity();
			var existing = FindEntity(entity);
			if (existing != null)
			{
				throw new Exception("Could not add newly created Entity to the Entity collection.");
			}
			mEntities.Add(entity.InstanceID, entity);
			return entity;
		}

		public IEntity FindEntity(IEntity entity)
		{
			if (entity == null)
			{
				return null;
			}
			return FindEntity(entity.InstanceID);
		}

		public IEntity FindEntity(Guid instanceID)
		{
			IEntity entity;
			mEntities.TryGetValue(instanceID, out entity);
			return entity;
		}

		public IEntity RemoveEntity(IEntity entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity), "Cannot remove invalid Entities");
			}

			if (!mEntities.Remove(entity.InstanceID))
			{
				return null;
			}

			return entity;

		}

		public IEntity RemoveEntity(Guid instanceID)
		{
			IEntity entity = FindEntity(instanceID);
			return RemoveEntity(entity);
		}

		//public bool AddEntity(IEntity entity)
		//{
		//	if(entity == null)
		//	{
		//		return false;
		//	}

		//	var existingEntity = FindEntity(entity);
		//	if(existingEntity == null)
		//	{
		//		return false;
		//	}

		//	mEntities.Add(entity.InstanceID, entity);
		//	return true;
		//}
	}
}
