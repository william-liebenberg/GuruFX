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
		public string Name { get; set; } = string.Empty;

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
		public IEntity FindEntity(IEntity entity, bool recurse) => Parent?.FindEntity(entity, recurse);
		public IEntity FindEntity(Guid instanceID, bool recurse) => Parent?.FindEntity(instanceID, recurse);
		public IEntity RemoveEntity(IEntity entity) => Parent?.RemoveEntity(entity);
		public IEntity RemoveEntity(Guid instanceID) => Parent?.RemoveEntity(instanceID);


		public bool AddComponent(IComponent component) => Parent?.AddComponent(component) ?? false;
		public T CreateAndAddComponent<T>() where T : class, IComponent, new() => Parent?.CreateAndAddComponent<T>();
		public IComponent CreateAndAddComponentOfType(Type componentType) => Parent?.CreateAndAddComponentOfType(componentType);
		public IComponent FindComponent(IComponent component, bool recurse) => Parent?.FindComponent(component, recurse);
		public IComponent FindComponent(Guid instanceID, bool recurse) => Parent?.FindComponent(instanceID, recurse);
		public IComponent RemoveComponent(IComponent component) => Parent?.RemoveComponent(component);
		public IComponent RemoveComponent(Guid instanceID) => Parent?.RemoveComponent(instanceID);
		

		public IComponent GetComponent<T>() where T : IComponent => GetComponent<T>((IComponent)null);
		public IComponent GetComponent<T>(IComponent excludedComponent) where T : IComponent => GetComponent<T>(new[] { excludedComponent });
		public IComponent GetComponent<T>(IComponent[] excludedComponents) where T : IComponent => Parent?.GetComponent<T>(excludedComponents);
		

		public IComponent[] GetComponents<T>() where T : IComponent => GetComponents<T>((IComponent)null);
		public IComponent[] GetComponents<T>(IComponent excludedComponent) where T : IComponent => GetComponents<T>(new[] { excludedComponent });
		public IComponent[] GetComponents<T>(IComponent[] excludedComponents) where T : IComponent => Parent?.GetComponents<T>(excludedComponents);
		

		public IComponent GetComponentFromParents<T>() where T : IComponent => GetComponentFromParents<T>((IComponent[])null);
		public IComponent GetComponentFromParents<T>(IComponent excludedComponent) where T : IComponent => GetComponentFromParents<T>(new[] { excludedComponent });
		public IComponent GetComponentFromParents<T>(IComponent[] excludedComponents) where T : IComponent => Parent?.GetComponentFromParents<T>(excludedComponents);


		public IComponent[] GetComponentsFromParents<T>() where T : IComponent => GetComponentsFromParents<T>((IComponent[])null);
		public IComponent[] GetComponentsFromParents<T>(IComponent excludedComponent) where T : IComponent => GetComponentsFromParents<T>(new[] { excludedComponent });
		public IComponent[] GetComponentsFromParents<T>(IComponent[] excludedComponents) where T : IComponent => Parent?.GetComponentsFromParents<T>(excludedComponents);


		public IComponent GetComponentFromChildren<T>() where T : IComponent => GetComponentFromChildren<T>((IComponent) null);
		public IComponent GetComponentFromChildren<T>(IComponent excludedComponent) where T : IComponent => GetComponentFromChildren<T>(new[] {excludedComponent});
		public IComponent GetComponentFromChildren<T>(IComponent[] excludedComponents) where T : IComponent => Parent?.GetComponentFromChildren<T>(excludedComponents);


		public IComponent[] GetComponentsFromChildren<T>() where T : IComponent => GetComponentsFromChildren<T>((IComponent)null);
		public IComponent[] GetComponentsFromChildren<T>(IComponent excludedComponent) where T : IComponent => GetComponentsFromParents<T>(new[] {excludedComponent});
		public IComponent[] GetComponentsFromChildren<T>(IComponent[] excludedComponents) where T : IComponent => Parent?.GetComponentsFromChildren<T>(excludedComponents);
		
		#endregion Shortcuts to Parent Entity Methods
	}
}
