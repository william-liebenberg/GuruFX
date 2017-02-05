using System;

namespace GuruFX.Core
{
	public interface IEntityComponentBase
	{
		bool AddEntity(IEntity entity);
		T CreateAndAddEntity<T>() where T : class, IEntity, new();
		IEntity CreateAndAddEntityOfType(Type entityType);
		IEntity FindEntity(IEntity entity, bool recurse);
		IEntity FindEntity(Guid instanceID, bool recurse);
		IEntity RemoveEntity(IEntity entity);
		IEntity RemoveEntity(Guid instanceID);


		bool AddComponent(IComponent component);
		T CreateAndAddComponent<T>() where T : class, IComponent, new();
		IComponent CreateAndAddComponentOfType(Type componentType);
		IComponent FindComponent(IComponent component, bool recurse);
		IComponent FindComponent(Guid instanceID, bool recurse);
		IComponent RemoveComponent(IComponent component);
		IComponent RemoveComponent(Guid instanceID);


		IComponent GetComponent<T>() where T : IComponent;
		IComponent GetComponent<T>(IComponent[] excludedComponents) where T : IComponent;
		IComponent GetComponent<T>(IComponent excludedComponent) where T : IComponent;
		IComponent GetComponentFromChildren<T>() where T : IComponent;
		IComponent GetComponentFromChildren<T>(IComponent[] excludedComponents) where T : IComponent;
		IComponent GetComponentFromChildren<T>(IComponent excludedComponent) where T : IComponent;
		IComponent GetComponentFromParents<T>() where T : IComponent;
		IComponent GetComponentFromParents<T>(IComponent[] excludedComponents) where T : IComponent;
		IComponent GetComponentFromParents<T>(IComponent excludedComponent) where T : IComponent;
		IComponent[] GetComponents<T>() where T : IComponent;
		IComponent[] GetComponents<T>(IComponent[] excludedComponents) where T : IComponent;
		IComponent[] GetComponents<T>(IComponent excludedComponent) where T : IComponent;
		IComponent[] GetComponentsFromChildren<T>() where T : IComponent;
		IComponent[] GetComponentsFromChildren<T>(IComponent[] excludedComponents) where T : IComponent;
		IComponent[] GetComponentsFromChildren<T>(IComponent excludedComponent) where T : IComponent;
		IComponent[] GetComponentsFromParents<T>() where T : IComponent;
		IComponent[] GetComponentsFromParents<T>(IComponent[] excludedComponents) where T : IComponent;
		IComponent[] GetComponentsFromParents<T>(IComponent excludedComponent) where T : IComponent;
	}
}