using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace GuruFX.Core
{
	public class Scene : IScene
	{
		readonly Dictionary<Guid, IEntity> mEntities = new Dictionary<Guid, IEntity>();

		public IEntity CreateEntity()
		{
			var entity = new Entity();
			mEntities.Add(entity.InstanceID, entity);
			return entity;
		}

		public IEntity FindEntity(IEntity entity)
		{
			if(entity == null)
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

		public bool AddEntity(IEntity entity)
		{
			if(entity == null)
			{
				return false;
			}

			var existingEntity = FindEntity(entity);
			if(existingEntity == null)
			{
				return false;
			}

			mEntities.Add(entity.InstanceID, entity);
			return true;
		}
	}
}
