namespace GuruFX.Core.Extensions
{
	public static class EntityExtensions
	{
		///// <summary>
		///// Find the child entity.
		///// </summary>
		///// <param name="entity"></param>
		///// <param name="entityToFind">The child entity to find.</param>
		///// <returns>The given entity, otherwise if the entity was not found to be a child of this entity then null is returned.</returns>
		//public static IEntity FindEntity(this IEntity entity, IEntity entityToFind) => entityToFind == null ? null : entity.FindEntity(entityToFind.InstanceID);

		///// <summary>
		///// Find a child entity via its InstanceID.
		///// </summary>
		///// <param name="entity"></param>
		///// <param name="instanceID">The instanceID of the child entity to find.</param>
		///// <returns>If found the child entity, otherwise null.</returns>
		//public static IEntity FindEntity(this IEntity entity, Guid instanceID)
		//{
		//	IEntity foundEntity;
		//	return entity.Entities.TryGetValue(instanceID, out foundEntity) ? entity : null;
		//}
	}
}
