using System;

namespace GuruFX.Core
{
	public interface IEntityComponentBase
	{
		bool AddEntity(IEntity entity);
		T CreateAndAddEntity<T>() where T : class, IEntity, new();
		IEntity CreateAndAddEntityOfType(Type entityType);
		IEntity FindEntity(IEntity entity);
		IEntity FindEntity(Guid instanceID);
		IEntity FindEntityFromChildren(IEntity entity);
		IEntity FindEntityFromChildren(Guid instanceID);
		IEntity RemoveEntity(IEntity entity);
		IEntity RemoveEntity(Guid instanceID);
		IEntity[] GetEntities();
		IComponent[] GetComponents();

		bool AddComponent(IComponent component);
		bool AddComponents(params IComponent[] components);

		T CreateAndAddComponent<T>() where T : class, IComponent, new();
		IComponent CreateAndAddComponentOfType(Type componentType);
		IComponent FindComponent(IComponent component);
		IComponent FindComponent(Guid instanceID);
		IComponent FindComponentFromChildren(IComponent component);
		IComponent FindComponentFromChildren(Guid instanceID);
		IComponent RemoveComponent(IComponent component);
		IComponent RemoveComponent(Guid instanceID);
		

		T GetComponent<T>() where T : class, IComponent;
		T GetComponent<T>(IComponent[] excludedComponents) where T : class, IComponent;
		T GetComponent<T>(IComponent excludedComponent) where T : class, IComponent;
		T GetComponentFromChildren<T>() where T : class, IComponent;
		T GetComponentFromChildren<T>(IComponent[] excludedComponents) where T : class, IComponent;
		T GetComponentFromChildren<T>(IComponent excludedComponent) where T : class, IComponent;
		T GetComponentFromParents<T>() where T : class, IComponent;
		T GetComponentFromParents<T>(IComponent[] excludedComponents) where T : class, IComponent;
		T GetComponentFromParents<T>(IComponent excludedComponent) where T : class, IComponent;
		T[] GetComponents<T>() where T : class, IComponent;
		T[] GetComponents<T>(IComponent[] excludedComponents) where T : class, IComponent;
		T[] GetComponents<T>(IComponent excludedComponent) where T : class, IComponent;
		T[] GetComponentsFromChildren<T>() where T : class, IComponent;
		T[] GetComponentsFromChildren<T>(IComponent[] excludedComponents) where T : class, IComponent;
		T[] GetComponentsFromChildren<T>(IComponent excludedComponent) where T : class, IComponent;
		T[] GetComponentsFromParents<T>() where T : class, IComponent;
		T[] GetComponentsFromParents<T>(IComponent[] excludedComponents) where T : class, IComponent;
		T[] GetComponentsFromParents<T>(IComponent excludedComponent) where T : class, IComponent;
	}
}