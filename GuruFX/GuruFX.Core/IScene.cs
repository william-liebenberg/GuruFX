using System;

namespace GuruFX.Core
{
	public interface IScene : IEntityFactory
	{
		IEntity FindEntity(IEntity entity);
		IEntity FindEntity(Guid instanceID);
		IEntity RemoveEntity(IEntity entity);
		IEntity RemoveEntity(Guid instanceID);
	}
}