using System;

namespace GuruFX.Core.Components
{
	public abstract class Component : IComponent
	{
		protected Component()
		{
			
		}

		protected Component(IEntity parent)
		{
			if (parent == null)
			{
				throw new ArgumentNullException(nameof(parent), "Parent Entity cannot be invalid!");
			}

			Parent = parent;
		}

		/// <summary>
		/// The Parent Entity. Components are always attached to an Entity.
		/// </summary>
		public IEntity Parent { get; set; }

		/// <summary>
		/// Name of this Component
		/// </summary>
		public abstract string Name { get; }

		/// <summary>
		/// Instance GUID of this Component
		/// </summary>
		public Guid InstanceID { get; set; } = Guid.NewGuid();

		/// <summary>
		/// The Component can be set to Active or Inactive.
		/// </summary>
		public bool IsActive { get; set; } = true;
		

		#region Shortcuts to Parent IEntityComponent Methods

		public bool AddEntity(IEntity entity) => Parent?.AddEntity(entity) ?? false;
		public T CreateAndAddEntity<T>() where T : class, IEntity, new() => Parent?.CreateAndAddEntity<T>();
		public IEntity CreateAndAddEntityOfType(Type entityType) => Parent?.CreateAndAddEntityOfType(entityType);
		public IEntity FindEntity(IEntity entity) => Parent?.FindEntity(entity);
		public IEntity FindEntity(Guid instanceID) => Parent?.FindEntity(instanceID);
		public IEntity FindEntityFromChildren(IEntity entity) => Parent?.FindEntityFromChildren(entity);
		public IEntity FindEntityFromChildren(Guid instanceID) => Parent?.FindEntityFromChildren(instanceID);
		public IEntity RemoveEntity(IEntity entity) => Parent?.RemoveEntity(entity);
		public IEntity RemoveEntity(Guid instanceID) => Parent?.RemoveEntity(instanceID);


		public bool AddComponent(IComponent component) => Parent?.AddComponent(component) ?? false;
		public bool AddComponents(params IComponent[] components) => Parent?.AddComponents(components) ?? false;
		public T CreateAndAddComponent<T>() where T : class, IComponent, new() => Parent?.CreateAndAddComponent<T>();
		public IComponent CreateAndAddComponentOfType(Type componentType) => Parent?.CreateAndAddComponentOfType(componentType);
		public IComponent FindComponent(IComponent component) => Parent?.FindComponent(component);
		public IComponent FindComponent(Guid instanceID) => Parent?.FindComponent(instanceID);
		public IComponent FindComponentFromChildren(IComponent component) => Parent?.FindComponentFromChildren(component);
		public IComponent FindComponentFromChildren(Guid instanceID) => Parent?.FindComponentFromChildren(instanceID);
		public IComponent RemoveComponent(IComponent component) => Parent?.RemoveComponent(component);
		public IComponent RemoveComponent(Guid instanceID) => Parent?.RemoveComponent(instanceID);
		

		public T GetComponent<T>() where T : class, IComponent => GetComponent<T>((T)null);
		public T GetComponent<T>(IComponent excludedComponent) where T : class, IComponent => GetComponent<T>(new[] { excludedComponent });
		public T GetComponent<T>(IComponent[] excludedComponents) where T : class, IComponent => Parent?.GetComponent<T>(excludedComponents);
		

		public T[] GetComponents<T>() where T : class, IComponent => GetComponents<T>((T)null);
		public T[] GetComponents<T>(IComponent excludedComponent) where T : class, IComponent => GetComponents<T>(new[] { excludedComponent });
		public T[] GetComponents<T>(IComponent[] excludedComponents) where T : class, IComponent => Parent?.GetComponents<T>(excludedComponents);
		

		public T GetComponentFromParents<T>() where T : class, IComponent => GetComponentFromParents<T>((T[])null);
		public T GetComponentFromParents<T>(IComponent excludedComponent) where T : class, IComponent => GetComponentFromParents<T>(new[] { excludedComponent });
		public T GetComponentFromParents<T>(IComponent[] excludedComponents) where T : class, IComponent => Parent?.GetComponentFromParents<T>(excludedComponents);


		public T[] GetComponentsFromParents<T>() where T : class, IComponent => GetComponentsFromParents<T>((T[])null);
		public T[] GetComponentsFromParents<T>(IComponent excludedComponent) where T : class, IComponent => GetComponentsFromParents<T>(new[] { excludedComponent });
		public T[] GetComponentsFromParents<T>(IComponent[] excludedComponents) where T : class, IComponent => Parent?.GetComponentsFromParents<T>(excludedComponents);


		public T GetComponentFromChildren<T>() where T : class, IComponent => GetComponentFromChildren<T>((T) null);
		public T GetComponentFromChildren<T>(IComponent excludedComponent) where T : class, IComponent => GetComponentFromChildren<T>(new[] {excludedComponent});
		public T GetComponentFromChildren<T>(IComponent[] excludedComponents) where T : class, IComponent => Parent?.GetComponentFromChildren<T>(excludedComponents);


		public T[] GetComponentsFromChildren<T>() where T : class, IComponent => GetComponentsFromChildren<T>((T)null);
		public T[] GetComponentsFromChildren<T>(IComponent excludedComponent) where T : class, IComponent => GetComponentsFromParents<T>(new[] {excludedComponent});
		public T[] GetComponentsFromChildren<T>(IComponent[] excludedComponents) where T : class, IComponent => Parent?.GetComponentsFromChildren<T>(excludedComponents);

		public IEntity[] GetEntities() => Parent?.GetEntities();

		public IComponent[] GetComponents() => Parent.GetComponents();

		#endregion Shortcuts to Parent Entity Methods
	}
}
