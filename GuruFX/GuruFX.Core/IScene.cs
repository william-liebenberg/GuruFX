using System;

namespace GuruFX.Core
{
	public interface IScene
	{
		bool AddEntity(IEntity entity);
		IEntity CreateEntity();
		IEntity FindEntity(IEntity entity);
		IEntity FindEntity(Guid instanceID);
	}
}